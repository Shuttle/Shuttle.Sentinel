using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterEndpoint
    {
        public RegisterEndpoint()
        {
            MessageTypeMetrics = new List<MessageTypeMetric>();
            MessageTypesHandled = new List<string>();
            MessageTypesDispatched = new List<Dispatched>();
            MessageTypeAssociations = new List<Association>();
        }

        public string EntryAssemblyQualifiedName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string IPv4Address { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string InboxDeferredQueueUri { get; set; }
        public string InboxErrorQueueUri { get; set; }
        public string OutboxWorkQueueUri { get; set; }
        public string OutboxErrorQueueUri { get; set; }
        public string ControlInboxWorkQueueUri { get; set; }
        public string ControlInboxErrorQueueUri { get; set; }
        public string HeartbeatIntervalDuration { get; set; }

        public List<Association> MessageTypeAssociations { get; set; }
        public List<Dispatched> MessageTypesDispatched { get; set; }
        public List<string> MessageTypesHandled { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MessageTypeMetric> MessageTypeMetrics { get; set; }

        public class Association
        {
            public string MessageTypeHandled { get; set; }
            public string MessageTypeDispatched { get; set; }
        }

        public class Dispatched
        {
            public string MessageType { get; set; }
            public string RecipientInboxWorkQueueUri { get; set; }
        }

        public class MessageTypeMetric
        {
            public string MessageType { get; set; }
            public int Count { get; set; }
            public double TotalExecutionDuration { get; set; }
            public double FastestExecutionDuration { get; set; }
            public double SlowestExecutionDuration { get; set; }
        }
    }
}
