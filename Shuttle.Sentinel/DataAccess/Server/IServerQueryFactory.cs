using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IServerQueryFactory
    {
        IQuery FindId(string machineName, string baseDirectory);
        IQuery Save(Guid id, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
        IQuery Add(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
    }
}