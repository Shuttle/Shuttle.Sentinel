namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterQueueCommand
    {
        public string QueueUri { get; set; }
        public string Processor { get; set; }
        public string Type { get; set; }
    }
}