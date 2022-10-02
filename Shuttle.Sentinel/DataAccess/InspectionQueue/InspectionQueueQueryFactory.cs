using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Streams;
using Shuttle.Esb;

namespace Shuttle.Sentinel.DataAccess
{
    public class InspectionQueueQueryFactory : IInspectionQueueQueryFactory
    {
        public IQuery Enqueue(string sourceQueueUri, TransportMessage transportMessage, Stream stream)
        {
            return RawQuery.Create(@"insert into [dbo].[InspectionQueue] (SourceQueueUri, MessageId, MessageBody) values (@SourceQueueUri, @MessageId, @MessageBody)")
                .AddParameterValue(Columns.SourceQueueUri, sourceQueueUri)
                .AddParameterValue(Columns.MessageId, transportMessage.MessageId)
                .AddParameterValue(Columns.MessageBody, stream.ToBytes());
        }

        public IQuery Messages()
        {
            return RawQuery.Create(@"select SourceQueueUri, MessageId, MessageBody from [dbo].[InspectionQueue]");
        }

        public IQuery Remove(Guid messageId)
        {
            return RawQuery.Create(@"delete from [dbo].[InspectionQueue] where MessageId = @MessageId")
                .AddParameterValue(Columns.MessageId, messageId);
        }

        public IQuery Get(Guid messageId)
        {
            return RawQuery.Create(@"select SourceQueueUri, MessageId, MessageBody from [dbo].[InspectionQueue]")
                .AddParameterValue(Columns.MessageId, messageId);
        }
    }
}