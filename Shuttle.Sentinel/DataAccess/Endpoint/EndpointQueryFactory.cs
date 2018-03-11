using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class EndpointQueryFactory : IEndpointQueryFactory
    {
        public IQuery FindId(string endpointName, string machineName, string baseDirectory)
        {
            return RawQuery.Create(@"
select
    Id
from
    Endpoint
where
    EndpointName = @EndpointName
and
    MachineName = @MachineName
and
    BaseDirectory = @BaseDirectory    
")
                .AddParameterValue(EndpointColumns.EndpointName, endpointName)
                .AddParameterValue(EndpointColumns.MachineName, machineName)
                .AddParameterValue(EndpointColumns.BaseDirectory, baseDirectory);
        }

        public IQuery Save(Guid id, string entryAssemblyQualifiedName, string inboxWorkQueueUri,
            string controlInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
update
    Endpoint
set
    EntryAssemblyQualifiedName = @EntryAssemblyQualifiedName,
    InboxWorkQueueUri = @InboxWorkQueueUri,
    ControlInboxWorkQueueUri = @ControlInboxWorkQueueUri
where
    Id = @Id
")
                .AddParameterValue(Columns.Id, id)
                .AddParameterValue(EndpointColumns.EntryAssemblyQualifiedName, entryAssemblyQualifiedName)
                .AddParameterValue(EndpointColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri);
        }

        public IQuery Add(string endpointName, string machineName, string baseDirectory,
            string entryAssemblyQualifiedName, string inboxWorkQueueUri, string controlInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
insert into Endpoint
(
    EndpointName,
    MachineName,
    BaseDirectory,
    EntryAssemblyQualifiedName,
    InboxWorkQueueUri,
    ControlInboxWorkQueueUri
)
values
(
    @EndpointName,
    @MachineName,
    @BaseDirectory,
    @EntryAssemblyQualifiedName,
    @InboxWorkQueueUri,
    @ControlInboxWorkQueueUri
)
")
                .AddParameterValue(EndpointColumns.EndpointName, endpointName)
                .AddParameterValue(EndpointColumns.MachineName, machineName)
                .AddParameterValue(EndpointColumns.BaseDirectory, baseDirectory)
                .AddParameterValue(EndpointColumns.EntryAssemblyQualifiedName, entryAssemblyQualifiedName)
                .AddParameterValue(EndpointColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri);
        }
    }
}