using System.IO;
using Shuttle.Core.Data;
using Shuttle.Esb;

namespace Shuttle.Sentinel.InspectionQueue
{
    public interface IInspectionQueueQueryFactory
    {
        IQuery Enqueue(TransportMessage transportMessage, Stream stream);
    }
}