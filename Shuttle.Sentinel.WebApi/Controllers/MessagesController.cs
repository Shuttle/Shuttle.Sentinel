using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Xml;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Queues;

namespace Shuttle.Sentinel.WebApi
{
    public class MessagesController : SentinelApiController
    {
        private readonly Type _transportMessageType = typeof(TransportMessage);
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IInspectionQueue _inspectionQueue;
        private readonly ISerializer _serializer;
        private readonly QueueManager _queueManager;

        [RequiresPermission(SystemPermissions.View.Users)]
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
                Message = Beautify(Encoding.UTF8.GetString(transportMessage.Message)),
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

        public MessagesController(IDatabaseContextFactory databaseContextFactory, IInspectionQueue inspectionQueue, ISerializer serializer)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(inspectionQueue, "inspectionQueue");
            Guard.AgainstNull(serializer, "serializer");

            _databaseContextFactory = databaseContextFactory;
            _inspectionQueue = inspectionQueue;
            _serializer = serializer;

            _queueManager = new QueueManager();
            _queueManager.ScanForQueueFactories();

        }

        [RequiresPermission(SystemPermissions.Manage.Roles)]
        [Route("api/messages/fetch")]
        public IHttpActionResult Fetch([FromBody] FetchMessageModel model)
        {
            try
            {
                var queue = _queueManager.CreateQueue(model.QueueUri);
                var countRetrieved = 0;

                try
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

                        _inspectionQueue.Enqueue(transportMessage, receivedMessage.Stream);

                        queue.Acknowledge(receivedMessage.AcknowledgementToken);
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

        private TransportMessage GetTransportMessage(Stream stream)
        {
            return (TransportMessage)_serializer.Deserialize(_transportMessageType, stream);
        }

        private string Beautify(string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }
    }
}