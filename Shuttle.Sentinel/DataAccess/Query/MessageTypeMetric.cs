using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class MessageTypeMetric
    {
        public string EnvironmentName { get; set; }
        public string MessageType { get; set; }
        public int Count { get; set; }
        public double TotalExecutionDuration { get; set; }
        public double FastestExecutionDuration { get; set; }
        public double SlowestExecutionDuration { get; set; }
        public double AverageExecutionDuration { get; set; }

        public class Specification
        {
            public Specification(DateTime startDateRegistered)
            {
                StartDateRegistered = startDateRegistered;
            }

            public DateTime StartDateRegistered { get; }
            public string MessageTypeMatch { get; private set; }

            public Specification MatchingMessageType(string messageTypeMatch)
            {
                MessageTypeMatch = messageTypeMatch;

                return this;
            }
        }
    }
}