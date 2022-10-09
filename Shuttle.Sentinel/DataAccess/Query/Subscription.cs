namespace Shuttle.Sentinel.DataAccess.Query
{
    public class Subscription
    {
        public string EnvironmentName { get; set; }
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}