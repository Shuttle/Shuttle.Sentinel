namespace Shuttle.Sentinel.DataAccess.Query
{
    public class MessageTypeHandled
    {
        public string EnvironmentName { get; set; }
        public string MessageType { get; set; }
        public int EndpointCount { get; set; }
    }
}