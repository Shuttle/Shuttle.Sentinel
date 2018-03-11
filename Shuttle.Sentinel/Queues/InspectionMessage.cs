using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Queues
{
    public class InspectionMessage
    {
        public InspectionMessage(string sourceQueueUri, Guid messageId, Stream stream)
        {
            Guard.AgainstNull(stream, nameof(stream));

            SourceQueueUri = sourceQueueUri;
            MessageId = messageId;
            Stream = stream;
        }

        public string SourceQueueUri { get; }
        public Guid MessageId { get; }
        public Stream Stream { get; }

        public TransportMessage TransportMessage(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            return (TransportMessage) serializer.Deserialize(typeof(TransportMessage), Stream);
        }
    }
}