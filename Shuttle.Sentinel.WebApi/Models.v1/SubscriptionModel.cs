namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class SubscriptionModel
    {
        public string MessageType { get; set; }
        public string InboxWorkQueueUri { get; set; }
    }
}