using System;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class EndpointQueryFactory : IEndpointQueryFactory
    {
        private const string SelectFrom = @"
select 
    Id, 
    MachineName,
    BaseDirectory,
    EnvironmentName,
    EntryAssemblyQualifiedName,
    IPv4Address,
    InboxWorkQueueUri,
    InboxDeferredQueueUri,
    InboxErrorQueueUri,
    ControlInboxWorkQueueUri,
    ControlInboxErrorQueueUri,
    OutboxWorkQueueUri,
    OutboxErrorQueueUri,
    HeartbeatIntervalDuration,
    HeartbeatDate,
    DateStarted,
    DateStopped
from 
    Endpoint
";

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
                .AddParameterValue(Columns.MachineName, machineName)
                .AddParameterValue(Columns.BaseDirectory, baseDirectory);
        }

        public IQuery Started(string machineName, string baseDirectory, string environmentName, string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string controlInboxWorkQueueUri, string controlInboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri, bool transientInstance, string heartbeatIntervalDuration, DateTime dateStarted)
        {
            return RawQuery.Create(@"
declare @date DateTime = getutcdate();

if not exists
(
    select
        null
    from
        Endpoint
    where
        MachineName = @MachineName
    and
        BaseDirectory = @BaseDirectory
)
    insert into Endpoint
    (
        MachineName,
        BaseDirectory,
        EnvironmentName,
        EntryAssemblyQualifiedName,
        IPv4Address,
        InboxWorkQueueUri,
        InboxDeferredQueueUri,
        InboxErrorQueueUri,
        ControlInboxWorkQueueUri,
        ControlInboxErrorQueueUri,
        OutboxWorkQueueUri,
        OutboxErrorQueueUri,
        TransientInstance,
        HeartbeatIntervalDuration,
        HeartbeatDate,
        DateStarted
    )
    values
    (
        @MachineName,
        @BaseDirectory,
        @EnvironmentName,
        @EntryAssemblyQualifiedName,
        @IPv4Address,
        @InboxWorkQueueUri,
        @InboxDeferredQueueUri,
        @InboxErrorQueueUri,
        @ControlInboxWorkQueueUri,
        @ControlInboxErrorQueueUri,
        @OutboxWorkQueueUri,
        @OutboxErrorQueueUri,
        @TransientInstance,
        @HeartbeatIntervalDuration,
        @date,
        @DateStarted
    )
else
begin
    update
        Endpoint
    set
        EnvironmentName = @EnvironmentName,
        EntryAssemblyQualifiedName = @EntryAssemblyQualifiedName,
        IPv4Address = @IPv4Address,
        InboxWorkQueueUri = @InboxWorkQueueUri,
        InboxDeferredQueueUri = @InboxDeferredQueueUri,
        InboxErrorQueueUri = @InboxErrorQueueUri,
        ControlInboxWorkQueueUri = @ControlInboxWorkQueueUri,
        ControlInboxErrorQueueUri = @ControlInboxErrorQueueUri,
        OutboxWorkQueueUri = @OutboxWorkQueueUri,
        OutboxErrorQueueUri = @OutboxErrorQueueUri,
        TransientInstance = @TransientInstance,
        HeartbeatDate = @date,
        DateStarted = @DateStarted
    where
        MachineName = @MachineName
    and
        BaseDirectory = @BaseDirectory
    and
        @DateStarted > DateStarted;
end
")
                .AddParameterValue(Columns.MachineName, machineName)
                .AddParameterValue(Columns.BaseDirectory, baseDirectory)
                .AddParameterValue(Columns.EnvironmentName, environmentName)
                .AddParameterValue(Columns.EntryAssemblyQualifiedName, entryAssemblyQualifiedName)
                .AddParameterValue(Columns.IPv4Address, ipv4Address)
                .AddParameterValue(Columns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(Columns.InboxErrorQueueUri, inboxErrorQueueUri)
                .AddParameterValue(Columns.InboxDeferredQueueUri, inboxDeferredQueueUri)
                .AddParameterValue(Columns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(Columns.ControlInboxErrorQueueUri, controlInboxErrorQueueUri)
                .AddParameterValue(Columns.OutboxWorkQueueUri, outboxWorkQueueUri)
                .AddParameterValue(Columns.OutboxErrorQueueUri, outboxErrorQueueUri)
                .AddParameterValue(Columns.HeartbeatIntervalDuration, heartbeatIntervalDuration)
                .AddParameterValue(Columns.TransientInstance, transientInstance)
                .AddParameterValue(Columns.DateStarted, dateStarted);
        }

        public IQuery Remove(Guid endpointId)
        {
            return RawQuery.Create(
                    @"delete from Endpoint where Id = @Id")
                .AddParameterValue(Columns.Id, endpointId);
        }

        public IQuery Search(string match)
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"
where 
(
    isnull(@Match, '') = ''
or
	MachineName like @Match
or
	BaseDirectory like @Match
or
	EntryAssemblyQualifiedName like @Match
or
	InboxWorkQueueUri like @Match
)
order by 
    MachineName
"))
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }

        public IQuery RegisterHeartbeat(Guid endpointId)
        {
            return RawQuery.Create(@"
update
    Endpoint
set
    HeartbeatDate = getutcdate()
where 
    Id = @Id
")
                .AddParameterValue(Columns.Id, endpointId);
        }

        public IQuery AddLogEntry(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
            int eventId, string scope)
        {
            return RawQuery.Create(@"
insert into EndpointLogEntry
(
	EndpointId,
	DateLogged,
	DateRegistered,
	Message,
    LogLevel,
    Category,
    EventId,
    Scope
)
values
(
	@Id,
	@DateLogged,
	@DateRegistered,
	@Message,
    @LogLevel,
    @Category,
    @EventId,
    @Scope
);
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.DateLogged, dateLogged)
                .AddParameterValue(Columns.DateRegistered, DateTime.UtcNow)
                .AddParameterValue(Columns.Message, message)
                .AddParameterValue(Columns.LogLevel, logLevel)
                .AddParameterValue(Columns.Category, category)
                .AddParameterValue(Columns.EventId, eventId)
                .AddParameterValue(Columns.Scope, scope);
        }

        public IQuery Stopped(Guid endpointId, DateTime dateStopped)
        {
            return RawQuery.Create(@"
    update
        Endpoint
    set
        DateStopped = @DateStopped
    where
        Id = @Id
    and
        @DateStopped > isnull(DateStopped, '0001-01-01');
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.DateStopped, dateStopped);
        }

        public IQuery RegisterSystemMetric(Guid endpointId, DateTime dateRegistered, string name, decimal value)
        {
            return RawQuery.Create(@"
insert into EndpointSystemMetric
(
	EndpointId,
	DateRegistered,
	Name,
	Value
)
values
(
	@Id,
	@DateRegistered,
	@Name,
	@Value
);
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.DateRegistered, dateRegistered)
                .AddParameterValue(Columns.Name, name)
                .AddParameterValue(Columns.Value, value);
        }
    }
}