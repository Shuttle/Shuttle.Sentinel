namespace Shuttle.Sentinel.Module
{
    public interface IMetricCollector
    {
        void SendMetrics();
        void AddExecutionDuration(string messageType, double duration);
    }
}