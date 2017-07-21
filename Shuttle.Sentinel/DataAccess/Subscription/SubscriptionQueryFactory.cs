using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class SubscriptionQueryFactory : ISubscriptionQueryFactory
    {
        public IQuery All()
        {
            return RawQuery.Create(@"
select
    MessageType,
    InboxWorkQueueUri
from
    SubscriberMessageType
order by
    MessageType
");
        }
    }
}