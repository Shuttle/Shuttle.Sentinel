using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface IEndpointQueryFactory
    {
        IQuery FindId(string endpointName, string machineName, string baseDirectory);
        IQuery Save(Guid id, string entryAssemblyQualifiedName, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
        IQuery Add(string endpointName, string machineName, string baseDirectory, string entryAssemblyQualifiedName, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
    }
}