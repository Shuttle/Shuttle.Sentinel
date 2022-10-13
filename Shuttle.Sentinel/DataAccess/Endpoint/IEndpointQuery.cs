using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IEndpointQuery
    {
        void Started(string machineName, string baseDirectory, string environmentName,
            string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string outboxWorkQueueUri, string outboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, bool transientInstance, string heartbeatIntervalDuration,
            DateTime dateStarted);

        Guid? FindId(string machineName, string baseDirectory);

        void AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration,
            double slowestExecutionDuration, double totalExecutionDuration);

        void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);
        void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType, string recipientInboxWorkQueueUri);
        void AddMessageTypeHandled(Guid endpointId, string messageType);
        void Remove(Guid endpointId);
        IEnumerable<Endpoint> Search(string match);
        void RegisterHeartbeat(Guid endpointId);
        void AddLogEntry(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
            int eventId, string scope);
        void Stopped(Guid endpointId, DateTime dateStopped);
        void RegisterSystemMetric(Guid endpointId, DateTime dateRegistered, string name, decimal value);
    }
}