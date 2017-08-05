namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public string InboxWorkQueueUri { get; set; }
        public string EndpointName { get; set; }
        public int HeartbeatIntervalSeconds { get; set; }
    }
}