namespace Shuttle.Sentinel.DataAccess.Query
{
    public class MessageTypeAssociation
    {
        public string MessageTypeHandled { get; set; }
        public string MessageTypeDispatched { get; set; }
        public int EndpointCount { get; set; }
    }
}