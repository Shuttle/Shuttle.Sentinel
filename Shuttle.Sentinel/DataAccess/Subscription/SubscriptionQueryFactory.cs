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

        public IQuery Add(Subscription subscription)
        {
            Guard.AgainstNull(subscription, nameof(subscription));

            return RawQuery.Create(@"
if not exists (select null from SubscriberMessageType where MessageType = @MessageType and InboxWorkQueueUri = @InboxWorkQueueUri)
    insert into SubscriberMessageType
    (
        MessageType,
        InboxWorkQueueUri
    ) 
    values 
    (
        @MessageType,
        @InboxWorkQueueUri
    )
")
                .AddParameterValue(Columns.MessageType, subscription.MessageType)
                .AddParameterValue(Columns.InboxWorkQueueUri, subscription.InboxWorkQueueUri);
        }

        public IQuery Remove(Subscription subscription)
        {
            Guard.AgainstNull(subscription, nameof(subscription));

            return RawQuery.Create(@"
delete 
from 
    SubscriberMessageType 
where 
    MessageType = @MessageType 
and 
    InboxWorkQueueUri = @InboxWorkQueueUri
")
                .AddParameterValue(Columns.MessageType, subscription.MessageType)
                .AddParameterValue(Columns.InboxWorkQueueUri, subscription.InboxWorkQueueUri);
        }
    }
}