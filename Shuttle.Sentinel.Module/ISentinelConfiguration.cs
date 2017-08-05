namespace Shuttle.Sentinel.Module
{
    public interface ISentinelConfiguration
    {
        string InboxWorkQueueUri { get; set; }
        string EndpointName { get; set; }
        int HeartbeatIntervalSeconds { get; set; }
    }
}