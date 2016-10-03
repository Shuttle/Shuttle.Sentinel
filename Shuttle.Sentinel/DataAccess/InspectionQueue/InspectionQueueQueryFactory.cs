using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.InspectionQueue
{
    public class InspectionQueueQueryFactory : IInspectionQueueQueryFactory
    {
        public IQuery Enqueue(TransportMessage transportMessage, Stream stream)
        {
            return RawQuery.Create(@"insert into [dbo].[InspectionQueue] (MessageId, MessageBody) values (@MessageId, @MessageBody)")
                .AddParameterValue(InspectionQueueColumns.MessageId, transportMessage.MessageId)
                .AddParameterValue(InspectionQueueColumns.MessageBody, stream.ToBytes());
        }

        public IQuery Messages()
        {
            return RawQuery.Create(@"select MessageId, MessageBody from [dbo].[InspectionQueue]");
        }

        public IQuery Remove(Guid messageId)
        {
            return RawQuery.Create(@"delete from [dbo].[InspectionQueue] where MessageId = @MessageId")
                .AddParameterValue(InspectionQueueColumns.MessageId, messageId);
        }
    }
}