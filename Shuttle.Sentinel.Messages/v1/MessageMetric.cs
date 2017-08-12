using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class MessageMetric
    {
        public string MessageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Count { get; set; }
        public int TotalExecutionDuration { get; set; }
        public int FastestExecutionDuration { get; set; }
        public int SlowestExecutionDuration { get; set; }
    }
}