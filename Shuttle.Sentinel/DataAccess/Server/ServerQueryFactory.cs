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

        public IQuery Save(Guid id, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
update
    Server
set
    IPv4Address = @IPv4Address,
    InboxWorkQueueUri = @InboxWorkQueueUri,
    ControlInboxWorkQueueUri = @ControlInboxWorkQueueUri
where
    Id = @Id
")
                .AddParameterValue(ServerColumns.IPv4Address, ipv4Address)
                .AddParameterValue(ServerColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(ServerColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(Columns.Id, id);
        }

        public IQuery Add(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri,
            string controlInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
insert into Server
(
    MachineName,
    BaseDirectory,
    IPv4Address,
    InboxWorkQueueUri,
    ControlInboxWorkQueueUri
)
values
(
    @MachineName,
    @BaseDirectory,
    @IPv4Address,
    @InboxWorkQueueUri,
    @ControlInboxWorkQueueUri
)
")
                .AddParameterValue(ServerColumns.MachineName, machineName)
                .AddParameterValue(ServerColumns.BaseDirectory, baseDirectory)
                .AddParameterValue(ServerColumns.IPv4Address, ipv4Address)
                .AddParameterValue(ServerColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(ServerColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri);
        }
    }
}