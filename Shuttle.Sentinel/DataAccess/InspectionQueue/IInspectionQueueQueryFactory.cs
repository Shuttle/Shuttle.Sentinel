using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Esb;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IInspectionQueueQueryFactory
    {
        IQuery Enqueue(string sourceQueueUri, TransportMessage transportMessage, Stream stream);
        IQuery Messages();
        IQuery Remove(Guid messageId);
        IQuery Get(Guid messageId);
    }
}