using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeAssociationQueryFactory : IMessageTypeAssociationQueryFactory
    {
        public IQuery Search(string match)
        {
            return RawQuery.Create(@"
select
	e.EnvironmentName,
    MessageTypeHandled,
    MessageTypeDispatched,
    count(*) EndpointCount
from
    EndpointMessageTypeAssociation mta
inner join
	[Endpoint] e on (e.Id = mta.EndpointId)
where 
(
    isnull(@Match, '') = ''
or
	MessageTypeHandled like @Match
or
    MessageTypeDispatched like @Match
)
group by
    e.EnvironmentName,
    MessageTypeHandled,
    MessageTypeDispatched
order by
    MessageTypeHandled
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }

        public IQuery Register(Guid endpointId, string messageTypeHandled, string messageTypeDispatched)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        EndpointMessageTypeAssociation
    where
        EndpointId = @Id
    and
        MessageTypeHandled = @MessageTypeHandled
    and
        MessageTypeDispatched = @MessageTypeDispatched
)
    insert into EndpointMessageTypeAssociation
    (
        EndpointId,
        MessageTypeHandled,
        MessageTypeDispatched,
        DateStamp
    )
    values
    (
        @Id,
        @MessageTypeHandled,
        @MessageTypeDispatched,
        @DateStamp
    )
else
    update 
        EndpointMessageTypeAssociation
    set
        DateStamp = @DateStamp
    where
        EndpointId = @Id
    and
        MessageTypeHandled = @MessageTypeHandled
    and
        MessageTypeDispatched = @MessageTypeDispatched
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageTypeHandled, messageTypeHandled)
                .AddParameterValue(Columns.MessageTypeDispatched, messageTypeDispatched)
                .AddParameterValue(Columns.DateStamp, DateTime.UtcNow);
        }
    }
}