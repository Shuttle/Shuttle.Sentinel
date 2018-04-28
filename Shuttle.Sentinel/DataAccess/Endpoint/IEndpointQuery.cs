using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IEndpointQuery
    {
        void Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName, string ipv4Address,
            string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string outboxWorkQueueUri, string outboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, string heartbeatIntervalDuration);

        Guid? FindId(string machineName, string baseDirectory);

        void AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration,
            double slowestExecutionDuration, double totalExecutionDuration);

        void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);
        void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType, string recipientInboxWorkQueueUri);
        void AddMessageTypeHandled(Guid endpointId, string messageType);
        void Remove(Guid id);
        IEnumerable<Endpoint> All();
        IEnumerable<Endpoint> Search(string match);
    }
}