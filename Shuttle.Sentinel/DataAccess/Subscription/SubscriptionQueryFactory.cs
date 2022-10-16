using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class SubscriptionQueryFactory : ISubscriptionQueryFactory
    {
        public IQuery All()
        {
            return RawQuery.Create(@"
select
    e.EnvironmentName,
    MessageType,
    InboxWorkQueueUri
from
    EndpointSubscription s
inner join
	[Endpoint] e on (e.Id = s.EndpointId)
order by
    e.EnvironmentName,
    MessageType
");
        }

        public IQuery Register(Guid endpointId, string messageType)
        {
            return RawQuery.Create(@"
if not exists 
(
    select 
        null 
    from 
        EndpointSubscription 
    where 
        EndpointId = @Id 
    and 
        MessageType = @MessageType
)
    insert into EndpointSubscription
    (
        EndpointId,
        MessageType,
        DateStamp
    )
    values
    (
        @Id,
        @MessageType,
        @DateStamp
    )
else
    update
        EndpointSubscription
    set
        DateStamp = @DateStamp
    where 
        EndpointId = @Id 
    and 
        MessageType = @MessageType
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageType, messageType)
                .AddParameterValue(Columns.DateStamp, DateTime.UtcNow);
        }
}
}