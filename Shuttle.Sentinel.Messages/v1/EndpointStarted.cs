using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class EndpointStarted
    {
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string IPv4Address { get; set; }
        public string EntryAssemblyQualifiedName { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string InboxDeferredQueueUri { get; set; }
        public string InboxErrorQueueUri { get; set; }
        public string OutboxWorkQueueUri { get; set; }
        public string OutboxErrorQueueUri { get; set; }
        public string ControlInboxWorkQueueUri { get; set; }
        public string ControlInboxErrorQueueUri { get; set; }
        public string HeartbeatIntervalDuration { get; set; }
        public List<string> Subscriptions { get; set; } = new List<string>();
        public bool TransientInstance { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
