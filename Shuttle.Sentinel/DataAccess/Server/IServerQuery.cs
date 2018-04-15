using System;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IServerQuery
    {
        Guid? FindId(string machineName, string baseDirectory);

        void Save(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri,
            string inboxDeferredQueueUri, string inboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri, string controlInboxWorkQueueUri, string controlInboxErrorQueueUri);
    }
}