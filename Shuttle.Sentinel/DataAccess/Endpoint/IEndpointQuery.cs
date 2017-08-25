namespace Shuttle.Sentinel
{
    public interface IEndpointQuery
    {
        void Register(string endpointName, string machineName, string baseDirectory, string entryAssemblyQualifiedName, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri);
    }
}