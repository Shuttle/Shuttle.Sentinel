using System;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IEndpointQuery
    {
        void Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName, string ipv4Address,
            string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string outboxWorkQueueUri, string outboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri);

        Guid? FindId(string machineName, string baseDirectory);

        void AddMessageTypeMetric(Guid endpointId, string messageType, int count, double fastestExecutionDuration,
            double slowestExecutionDuration, double totalExecutionDuration);

        void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);
        void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType, string recipientInboxWorkQueueUri);
        void AddMessageTypeHandled(Guid endpointId, string messageType);
    }
}