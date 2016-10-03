using System;
using System.IO;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Queues
{
    public class InspectionMessage
    {
        public Guid MessageId { get; private set; }
        public Stream Stream { get; private set; }

        public InspectionMessage(Guid messageId, Stream stream)
        {
            Guard.AgainstNull(stream, "stream");

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