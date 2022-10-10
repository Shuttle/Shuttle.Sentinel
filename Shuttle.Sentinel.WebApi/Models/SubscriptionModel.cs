using System;

namespace Shuttle.Sentinel.WebApi
{
    public class SubscriptionModel
    {
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}