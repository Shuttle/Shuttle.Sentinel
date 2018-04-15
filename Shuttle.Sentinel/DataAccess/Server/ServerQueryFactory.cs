using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class ServerQueryFactory : IServerQueryFactory
    {
        public IQuery FindId(string machineName, string baseDirectory)
        {
            return RawQuery.Create(@"
select
    Id
from
    Server
where
    MachineName = @MachineName
and
    BaseDirectory = @BaseDirectory
")
                .AddParameterValue(ServerColumns.MachineName, machineName)
                .AddParameterValue(ServerColumns.BaseDirectory, baseDirectory);
        }

        public IQuery Save(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri,
            string inboxDeferredQueueUri, string inboxErrorQueueUri,
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
    insert into Server
    (
        MachineName,
        BaseDirectory,
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
        Server
    set
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
                .AddParameterValue(ServerColumns.MachineName, machineName)
                .AddParameterValue(ServerColumns.BaseDirectory, baseDirectory)
                .AddParameterValue(ServerColumns.IPv4Address, ipv4Address)
                .AddParameterValue(ServerColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(ServerColumns.InboxErrorQueueUri, inboxErrorQueueUri)
                .AddParameterValue(ServerColumns.InboxDeferredQueueUri, inboxDeferredQueueUri)
                .AddParameterValue(ServerColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(ServerColumns.ControlInboxErrorQueueUri, controlInboxErrorQueueUri)
                .AddParameterValue(ServerColumns.OutboxWorkQueueUri, outboxWorkQueueUri)
                .AddParameterValue(ServerColumns.OutboxErrorQueueUri, outboxErrorQueueUri);
        }
    }
}