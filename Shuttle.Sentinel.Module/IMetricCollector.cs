namespace Shuttle.Sentinel.Module
{
    public interface IMetricCollector
    {
        void SendMetrics();
        void AddExecutionDuration(string messageType, double duration);
        void AddMessageTypeDispatched(string messageTypeDispatched, string recipientInboxWorkQueueUri);
        void AddMessageTypeAssociation(string messageTypeHandled, string messageTypeDispatched);
    }
}