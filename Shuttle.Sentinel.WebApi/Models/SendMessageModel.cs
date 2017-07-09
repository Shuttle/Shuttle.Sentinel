namespace Shuttle.Sentinel.WebApi
{
    public class SendMessageModel
    {
        public string DestinationQueueUri { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
    }
}