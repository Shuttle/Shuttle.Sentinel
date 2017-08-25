using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMetricsCommand
    {
        public RegisterMetricsCommand()
        {
            SystemMetrics = new List<SystemMetric>();
            MessageMetrics = new List<MessageTypeMetric>();
            MessageTypesHandled = new List<string>();
            MessageTypesDispatched = new List<Dispatched>();
            MessageTypeAssociations = new List<Association>();
        }

        public List<Association> MessageTypeAssociations { get; set; }
        public List<Dispatched> MessageTypesDispatched { get; set; }
        public List<string> MessageTypesHandled { get; set; }

        public Guid MetricId { get; set; }
        public string EndpointName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SystemMetric> SystemMetrics { get; set; }
        public List<MessageTypeMetric> MessageMetrics { get; set; }

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
    }
}