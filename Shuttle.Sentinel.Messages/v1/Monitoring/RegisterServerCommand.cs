namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterServerCommand
    {
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string IPv4Address { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string ControlInboxWorkQueueUri { get; set; }
    }
}