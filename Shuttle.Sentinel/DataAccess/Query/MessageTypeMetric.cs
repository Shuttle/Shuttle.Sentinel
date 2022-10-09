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
    }
}