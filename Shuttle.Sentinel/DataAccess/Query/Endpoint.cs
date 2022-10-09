using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class Endpoint
    {
        public Guid Id { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string EnvironmentName { get; set; }
        public string EntryAssemblyQualifiedName { get; set; }
        public string Ipv4Address { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string InboxDeferredQueueUri { get; set; }
        public string InboxErrorQueueUri { get; set; }
        public string ControlInboxWorkQueueUri { get; set; }
        public string ControlInboxErrorQueueUri { get; set; }
        public string OutboxWorkQueueUri { get; set; }
        public string OutboxErrorQueueUri { get; set; }
        public DateTime HeartbeatDate { get; set; }
        public string HeartbeatIntervalDuration { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateStopped { get; set; }
        public bool TransientInstance { get; set; }
    }
}