using System.IO;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Queues
{
    public class InspectionMessage
    {
        public Stream Stream { get; private set; }

        public InspectionMessage(Stream stream)
        {
            Guard.AgainstNull(stream, "stream");

            Stream = stream;
        }

        public TransportMessage TransportMessage(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, "serializer");

            return (TransportMessage)serializer.Deserialize(typeof (TransportMessage), Stream);
        }
    }
}