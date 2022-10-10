using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class AddSubscription
    {
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}