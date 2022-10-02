using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypeMetrics : EndpointMessage
    {
        public List<MessageTypeMetric> MessageTypeMetrics { get; set; } = new List<MessageTypeMetric>();

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
