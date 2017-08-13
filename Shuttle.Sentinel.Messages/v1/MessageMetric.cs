namespace Shuttle.Sentinel.Messages.v1
{
    public class MessageMetric
    {
        public string MessageType { get; set; }
        public int Count { get; set; }
        public double TotalExecutionDuration { get; set; }
        public double FastestExecutionDuration { get; set; }
        public double SlowestExecutionDuration { get; set; }
    }
}