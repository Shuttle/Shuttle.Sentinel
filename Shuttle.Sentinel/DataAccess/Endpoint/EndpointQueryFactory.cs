using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class EndpointQueryFactory : IEndpointQueryFactory
    {
        public IQuery FindId(string machineName, string baseDirectory)
        {
            return RawQuery.Create(@"
select
    Id
from
    Endpoint
where
    MachineName = @MachineName
and
    BaseDirectory = @BaseDirectory    
")
                .AddParameterValue(EndpointColumns.MachineName, machineName)
                .AddParameterValue(EndpointColumns.BaseDirectory, baseDirectory);
        }

        public IQuery Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string controlInboxWorkQueueUri, string controlInboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        Server
    where
        MachineName = @MachineName
    and
        BaseDirectory = @BaseDirectory
)
    insert into Endpoint
    (
        MachineName,
        BaseDirectory,
        EntryAssemblyQualifiedName,
        IPv4Address,
        InboxWorkQueueUri,
        InboxDeferredQueueUri,
        InboxErrorQueueUri,
        ControlInboxWorkQueueUri,
        ControlInboxErrorQueueUri,
        OutboxWorkQueueUri,
        OutboxErrorQueueUri
    )
    values
    (
        @MachineName,
        @BaseDirectory,
        @EntryAssemblyQualifiedName,
        @IPv4Address,
        @InboxWorkQueueUri,
        @InboxDeferredQueueUri,
        @InboxErrorQueueUri,
        @ControlInboxWorkQueueUri,
        @ControlInboxErrorQueueUri,
        @OutboxWorkQueueUri,
        @OutboxErrorQueueUri    
    )
else
    update
        Endpoint
    set
        EntryAssemblyQualifiedName = @EntryAssemblyQualifiedName,
        IPv4Address = @IPv4Address,
        InboxWorkQueueUri = @InboxWorkQueueUri,
        InboxDeferredQueueUri = @InboxDeferredQueueUri,
        InboxErrorQueueUri = @InboxErrorQueueUri,
        ControlInboxWorkQueueUri = @ControlInboxWorkQueueUri,
        ControlInboxErrorQueueUri = @ControlInboxErrorQueueUri,
        OutboxWorkQueueUri = @OutboxWorkQueueUri,
        OutboxErrorQueueUri = @OutboxErrorQueueUri
    where
        MachineName = @MachineName
    and
        BaseDirectory = @BaseDirectory
")
                .AddParameterValue(EndpointColumns.MachineName, machineName)
                .AddParameterValue(EndpointColumns.BaseDirectory, baseDirectory)
                .AddParameterValue(EndpointColumns.IPv4Address, ipv4Address)
                .AddParameterValue(EndpointColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.InboxErrorQueueUri, inboxErrorQueueUri)
                .AddParameterValue(EndpointColumns.InboxDeferredQueueUri, inboxDeferredQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxErrorQueueUri, controlInboxErrorQueueUri)
                .AddParameterValue(EndpointColumns.OutboxWorkQueueUri, outboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.OutboxErrorQueueUri, outboxErrorQueueUri);
        }
    }
}