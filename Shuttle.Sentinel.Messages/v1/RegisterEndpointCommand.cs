namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterEndpointCommand
    {
        public string EndpointName { get; set; }
        public string EntryAssemblyQualifiedName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string IPv4Address { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string ControlInboxWorkQueueUri { get; set; }
    }
}
