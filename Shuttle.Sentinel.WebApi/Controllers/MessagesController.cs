using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Queues;

namespace Shuttle.Sentinel.WebApi
{
    [RequiresPermission(SystemPermissions.Manage.Messages)]
    public class MessagesController : SentinelApiController
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IInspectionQueue _inspectionQueue;
        private readonly IQueueManager _queueManager;
        private readonly ISerializer _serializer;
        private readonly ITransportMessageFactory _transportMessageFactory;
        private readonly Type _transportMessageType = typeof(TransportMessage);

        public MessagesController(IDatabaseContextFactory databaseContextFactory, IInspectionQueue inspectionQueue,
            ISerializer serializer, IQueueManager queueManager, ITransportMessageFactory transportMessageFactory)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(inspectionQueue, "inspectionQueue");
            Guard.AgainstNull(serializer, "serializer");
            Guard.AgainstNull(queueManager, "queueManager");
            Guard.AgainstNull(transportMessageFactory, "transportMessageFactory");

            _databaseContextFactory = databaseContextFactory;
            _inspectionQueue = inspectionQueue;
            _serializer = serializer;
            _queueManager = queueManager;
            _transportMessageFactory = transportMessageFactory;
        }

        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = from message in _inspectionQueue.Messages().ToList()
                    select PresentationMessage(message)
                });
            }
        }

        private dynamic PresentationMessage(InspectionMessage message)
        {
            var transportMessage = GetTransportMessage(message.Stream);

            return new
            {
                message.SourceQueueUri,
                Message = Encoding.UTF8.GetString(transportMessage.Message),
                transportMessage.AssemblyQualifiedName,
                transportMessage.CompressionAlgorithm,
                transportMessage.CorrelationId,
                transportMessage.EncryptionAlgorithm,
                ExpiryDate = transportMessage.ExpiryDate.ToUniversalTime(),
                transportMessage.FailureMessages,
                transportMessage.Headers,
                IgnoreTillDate = transportMessage.IgnoreTillDate.ToUniversalTime(),
                transportMessage.MessageId,
                transportMessage.MessageReceivedId,
                transportMessage.MessageType,
                transportMessage.PrincipalIdentityName,
                transportMessage.RecipientInboxWorkQueueUri,
                SendDate = transportMessage.SendDate.ToUniversalTime(),
                transportMessage.SenderInboxWorkQueueUri
            };
        }

        [Route("api/messages/fetch")]
        public IHttpActionResult Fetch([FromBody] FetchMessageModel model)
        {
            try
            {
                var queue = _queueManager.CreateQueue(model.QueueUri);
                var countRetrieved = 0;

                try
                {
                    for (var i = 0; i < model.Count; i++)
                    {
                        var receivedMessage = queue.GetMessage();

                        if (receivedMessage != null)
                        {
                            countRetrieved++;

                            TransportMessage transportMessage;

                            try
                            {
                                transportMessage = GetTransportMessage(receivedMessage.Stream);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }

                            _inspectionQueue.Enqueue(model.QueueUri, transportMessage, receivedMessage.Stream);

                            queue.Acknowledge(receivedMessage.AcknowledgementToken);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    queue.AttemptDispose();
                }

                return Ok(new
                {
                    Data = new
                    {
                        CountRetrieved = countRetrieved
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/messages/transfer")]
        public IHttpActionResult Transfer([FromBody] MessageMoveModel model)
        {
            try
            {
                IQueue queue = null;
                var previousQueueUri = string.Empty;

                foreach (var messageId in model.MessageIds)
                {
                    var action = model.Action.ToLower();

                    if (action.Equals("remove"))
                    {
                        using (_databaseContextFactory.Create())
                        {
                            _inspectionQueue.Remove(messageId);
                        }

                        continue;
                    }

                    var inspectionMessage = _inspectionQueue.Get(messageId);

                    TransportMessage transportMessage;

                    try
                    {
                        transportMessage = GetTransportMessage(inspectionMessage.Stream);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }

                    var queueUri = string.Empty;
                    var stream = inspectionMessage.Stream;

                    switch (action)
                    {
                        case "copy":
                        case "move":
                        {
                            queueUri = model.DestinationQueueUri;

                            break;
                        }
                        case "returntosourcequeue":
                        {
                            queueUri = inspectionMessage.SourceQueueUri;

                            break;
                        }
                        case "sendtorecipientqueue":
                        {
                            queueUri = transportMessage.RecipientInboxWorkQueueUri;

                            break;
                        }
                        case "stopignoring":
                        {
                            queueUri = transportMessage.RecipientInboxWorkQueueUri;

                            transportMessage.IgnoreTillDate = DateTime.MinValue;
                            stream = _serializer.Serialize(transportMessage);

                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(queueUri))
                    {
                        if (!queueUri.Equals(previousQueueUri))
                        {
                            queue?.AttemptDispose();

                            queue = _queueManager.CreateQueue(queueUri);

                            previousQueueUri = queueUri;
                        }

                        queue?.Enqueue(transportMessage, stream);

                        if (!action.Equals("copy"))
                        {
                            _inspectionQueue.Remove(messageId);
                        }
                    }
                }

                queue?.AttemptDispose();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult Post([FromBody] SendMessageModel model)
        {
            Guard.AgainstNull(model, "model");

            IQueue queue = null;

            try
            {
                queue = _queueManager.CreateQueue(model.DestinationQueueUri);

                var transportMessage = _transportMessageFactory.Create(new object(), c => c.WithRecipient(queue));

                transportMessage.AssemblyQualifiedName = model.MessageType;
                transportMessage.MessageType = model.MessageType;
                transportMessage.Message = Encoding.UTF8.GetBytes(model.Message);

                using (var stream = _serializer.Serialize(transportMessage))
                {
                    queue.Enqueue(transportMessage, stream);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                queue?.AttemptDispose();
            }

            return Ok();
        }

        private TransportMessage GetTransportMessage(Stream stream)
        {
            return (TransportMessage) _serializer.Deserialize(_transportMessageType, stream);
        }
    }
}