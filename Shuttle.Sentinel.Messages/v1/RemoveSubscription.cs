using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RemoveSubscription
    {
        public Guid DataStoreId { get; set; }
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}