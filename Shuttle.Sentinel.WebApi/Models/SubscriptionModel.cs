using System;

namespace Shuttle.Sentinel.WebApi
{
    public class SubscriptionModel
    {
        public Guid DataStoreId { get; set; }
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}