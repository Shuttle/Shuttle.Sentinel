using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IEndpointQueryFactory
    {
        IQuery FindId(string machineName, string baseDirectory);

        IQuery Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName, string ipv4Address,
            string inboxWorkQueueUri,
            string inboxDeferredQueueUri, string inboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri);

        IQuery AddMessageTypeHandled(Guid endpointId, string messageType);

        IQuery AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri);

        IQuery AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);

        IQuery AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration);

        IQuery Remove(Guid id);
        IQuery All();
        IQuery Search(string match);
    }
}