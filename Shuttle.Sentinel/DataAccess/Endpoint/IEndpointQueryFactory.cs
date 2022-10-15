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

        IQuery Remove(Guid endpointId);
        IQuery Search(string match);
        IQuery RegisterHeartbeat(Guid endpointId);
        IQuery Stopped(Guid endpointId, DateTime dateStopped);
        IQuery RegisterSystemMetric(Guid endpointId, DateTime dateRegistered, string name, decimal value);
    }
}