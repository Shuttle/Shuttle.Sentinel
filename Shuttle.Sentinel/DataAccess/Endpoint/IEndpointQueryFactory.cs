using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IEndpointQueryFactory
    {
        IQuery FindId(string machineName, string baseDirectory);

        IQuery Started(string machineName, string baseDirectory, string environmentName,
            string entryAssemblyQualifiedName, string ipv4Address,
            string inboxWorkQueueUri,
            string inboxDeferredQueueUri, string inboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri, bool transientInstance, string heartbeatIntervalDuration, DateTime dateStarted);

        IQuery AddMessageTypeHandled(Guid endpointId, string messageType);

        IQuery AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri);

        IQuery AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);

        IQuery AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration);

        IQuery Remove(Guid endpointId);
        IQuery All();
        IQuery Search(string match);
        IQuery RegisterHeartbeat(Guid endpointId);
        IQuery AddLogEntry(Guid endpointId, DateTime dateLogged, string message);
        IQuery Stopped(Guid endpointId, DateTime dateStopped);
        IQuery RegisterSystemMetric(Guid endpointId, DateTime dateRegistered, string name, decimal value);
    }
}