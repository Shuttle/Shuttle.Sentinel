﻿namespace Shuttle.Sentinel.Messages.v1
{
    public class RemoveSubscription
    {
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}