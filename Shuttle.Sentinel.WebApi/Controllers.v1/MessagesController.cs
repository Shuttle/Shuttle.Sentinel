using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;
using Shuttle.Core.Serialization;
using Shuttle.Esb;
using Shuttle.Sentinel.Queues;
using Shuttle.Sentinel.WebApi.Models.v1;

namespace Shuttle.Sentinel.WebApi.Controllers.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    [RequiresPermission(Permissions.Manage.Messages)]
    public class MessagesController : Controller
    {
        private readonly IPipelineFactory _pipelineFactory;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IInspectionQueue _inspectionQueue;
        private readonly ILog _log;
        private readonly IQueueService _queueService;
        private readonly ISerializer _serializer;
        private readonly Type _transportMessageType = typeof(TransportMessage);

        public MessagesController(IPipelineFactory pipelineFactory, IDatabaseContextFactory databaseContextFactory, IInspectionQueue inspectionQueue,
            ISerializer serializer, IQueueService queueService)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(inspectionQueue, nameof(inspectionQueue));
            Guard.AgainstNull(serializer, nameof(serializer));
            Guard.AgainstNull(queueService, nameof(queueService));

            _pipelineFactory = pipelineFactory;
            _databaseContextFactory = databaseContextFactory;
            _inspectionQueue = inspectionQueue;
            _serializer = serializer;
            _queueService = queueService;

            _log = Log.For(this);
        }

        [HttpGet]
        public IActionResult Get()
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

        [HttpPost("fetch")]
        public IActionResult Fetch([FromBody] FetchMessageModel model)
        {
            try
            {
                var queue = _queueService.Get(model.QueueUri);
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
                                _log.Error(ex.AllMessages());

                                return StatusCode((int) HttpStatusCode.InternalServerError, ex);
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

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] MessageTransferModel model)
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
                        _log.Error(ex.AllMessages());

                        return StatusCode((int) HttpStatusCode.InternalServerError, ex);
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

                            queue = _queueService.Get(queueUri);

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

        [HttpPost("transferdirect")]
        public IActionResult TransferDirect([FromBody] MessageTransferDirectModel model)
        {
            try
            {
                var sourceQueue = _queueService.Get(model.SourceQueueUri);
                var destinationQueue = _queueService.Get(model.DestinationQueueUri);
                var countRetrieved = 0;

                try
                {
                    for (var i = 0; i < model.Count; i++)
                    {
                        var receivedMessage = sourceQueue.GetMessage();

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
                                _log.Error(ex.AllMessages());

                                return StatusCode((int) HttpStatusCode.InternalServerError, ex);
                            }

                            destinationQueue.Enqueue(transportMessage, receivedMessage.Stream);

                            if (model.Action.Equals("Move", StringComparison.InvariantCultureIgnoreCase))
                            {
                                sourceQueue.Acknowledge(receivedMessage.AcknowledgementToken);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    sourceQueue.AttemptDispose();
                    destinationQueue.AttemptDispose();
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

        [HttpPost]
        public IActionResult Post([FromBody] SendMessageModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            IQueue queue = null;

            try
            {
                queue = _queueService.Get(model.DestinationQueueUri);

                var transportMessage = new TransportMessage
                {
                    AssemblyQualifiedName = model.MessageType,
                    MessageType = model.MessageType,
                    Message = Encoding.UTF8.GetBytes(model.Message),
                    RecipientInboxWorkQueueUri = queue.Uri.ToString()
                };

                foreach (var header in model.Headers)
                {
                    transportMessage.Headers.Add(new TransportHeader {Key = header.Key, Value = header.Value});
                }

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