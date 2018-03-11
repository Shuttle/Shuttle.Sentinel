using System;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IServerQuery
    {
        Guid? FindId(string machineName, string baseDirectory);
        void Save(Guid id, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
        void Add(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
    }
}