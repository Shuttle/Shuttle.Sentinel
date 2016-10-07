using System;
using System.IO;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Queues
{
    public class InspectionMessage
    {
        public string SourceQueueUri { get; private set; }
        public Guid MessageId { get; private set; }
        public Stream Stream { get; private set; }

        public InspectionMessage(string sourceQueueUri, Guid messageId, Stream stream)
        {
            Guard.AgainstNull(stream, "stream");

            SourceQueueUri = sourceQueueUri;
            MessageId = messageId;
            Stream = stream;
        }

        public TransportMessage TransportMessage(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, "serializer");

            return (TransportMessage)serializer.Deserialize(typeof (TransportMessage), Stream);
        }
    }
}