using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeDispatchedQueryFactory : IMessageTypeDispatchedQueryFactory
    {
        public IQuery Search(string match)
        {
            return RawQuery.Create(@"
select
    e.EnvironmentName,
    MessageType,
    RecipientInboxWorkQueueUri,
    count(*) EndpointCount
from
    EndpointMessageTypeDispatched mtd
inner join
	[Endpoint] e on (e.Id = mtd.EndpointId)
where 
(
    isnull(@Match, '') = ''
or
	MessageType like @Match
or
    RecipientInboxWorkQueueUri like @Match
)
group by
    e.EnvironmentName,
    MessageType,
    RecipientInboxWorkQueueUri
order by
    MessageType
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }

        public IQuery Register(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        EndpointMessageTypeDispatched
    where
        EndpointId = @Id
    and
        MessageType = @MessageType
    and
        RecipientInboxWorkQueueUri = @RecipientInboxWorkQueueUri 
)
    insert into EndpointMessageTypeDispatched
    (
        EndpointId,
        MessageType,
        RecipientInboxWorkQueueUri,
        DateStamp
    )
    values
    (
        @Id,
        @MessageType,
        @RecipientInboxWorkQueueUri,
        @DateStamp
    )
else
    update
        EndpointMessageTypeDispatched
    set
        DateStamp = @DateStamp
    where
        EndpointId = @Id
    and
        MessageType = @MessageType
    and
        RecipientInboxWorkQueueUri = @RecipientInboxWorkQueueUri 
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageType, dispatchedMessageType)
                .AddParameterValue(Columns.RecipientInboxWorkQueueUri, recipientInboxWorkQueueUri)
                .AddParameterValue(Columns.DateStamp, DateTime.UtcNow);
        }
    }
}