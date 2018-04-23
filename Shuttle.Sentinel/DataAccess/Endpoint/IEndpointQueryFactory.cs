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
    }
}