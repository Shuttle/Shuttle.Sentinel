using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.InspectionQueue
{
    public class InspectionQueueQueryFactoryFactory : IInspectionQueueQueryFactory
    {
        public IQuery Enqueue(TransportMessage transportMessage, Stream stream)
        {
            return RawQuery.Create(@"insert into [dbo].[{0}] (MessageId, MessageBody) values (@MessageId, @MessageBody)")
                .AddParameterValue(InspectionQueueColumns.MessageId, transportMessage.MessageId)
                .AddParameterValue(InspectionQueueColumns.MessageBody, stream.ToBytes());
        }
    }
}