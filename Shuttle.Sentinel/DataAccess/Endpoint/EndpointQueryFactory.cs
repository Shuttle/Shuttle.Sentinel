using System;
using System.Security.Claims;
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
    HeartbeatDate
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

        public IQuery Started(string machineName, string baseDirectory, string entryAssemblyQualifiedName,
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
        HeartbeatDate = @date
    where
        MachineName = @MachineName
    and
        BaseDirectory = @BaseDirectory;

    update
        Endpoint
    set
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
                .AddParameterValue(Columns.EntryAssemblyQualifiedName, entryAssemblyQualifiedName)
                .AddParameterValue(Columns.IPv4Address, ipv4Address)
                .AddParameterValue(Columns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(Columns.InboxErrorQueueUri, inboxErrorQueueUri)
                .AddParameterValue(Columns.InboxDeferredQueueUri, inboxDeferredQueueUri)
                .AddParameterValue(Columns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(Columns.ControlInboxErrorQueueUri, controlInboxErrorQueueUri)
                .AddParameterValue(Columns.OutboxWorkQueueUri, outboxWorkQueueUri)
                .AddParameterValue(Columns.OutboxErrorQueueUri, outboxErrorQueueUri)
                .AddParameterValue(Columns.OutboxErrorQueueUri, outboxErrorQueueUri)
                .AddParameterValue(Columns.HeartbeatIntervalDuration, heartbeatIntervalDuration)
                .AddParameterValue(Columns.DateStarted, dateStarted);
        }

        public IQuery AddMessageTypeHandled(Guid endpointId, string messageType)
        {
            return RawQuery.Create(@"
if not exists (select null from EndpointMessageTypeHandled where EndpointId = @Id and MessageType = @MessageType)
    insert into EndpointMessageTypeHandled
    (
        EndpointId,
        MessageType
    )
    values
    (
        @Id,
        @MessageType
    )
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageType, messageType);
        }

        public IQuery AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        EndpointMessageTypeDispatched
    where
        EndpointId = @Id
    and
        MessageType = @MessageType
    and
        RecipientInboxWorkQueueUri = @RecipientInboxWorkQueueUri 
)
    insert into EndpointMessageTypeDispatched
    (
        EndpointId,
        MessageType,
        RecipientInboxWorkQueueUri
    )
    values
    (
        @Id,
        @MessageType,
        @RecipientInboxWorkQueueUri
    )
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageType, dispatchedMessageType)
                .AddParameterValue(Columns.RecipientInboxWorkQueueUri, recipientInboxWorkQueueUri);
        }

        public IQuery AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled,
            string messageTypeDispatched)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        EndpointMessageTypeAssociation
    where
        EndpointId = @Id
    and
        MessageTypeHandled = @MessageTypeHandled
    and
        MessageTypeDispatched = @MessageTypeDispatched
)
    insert into EndpointMessageTypeAssociation
    (
        EndpointId,
        MessageTypeHandled,
        MessageTypeDispatched
    )
    values
    (
        @Id,
        @MessageTypeHandled,
        @MessageTypeDispatched
    )
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.MessageTypeHandled, messageTypeHandled)
                .AddParameterValue(Columns.MessageTypeDispatched, messageTypeDispatched);
        }

        public IQuery AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
            return RawQuery.Create(@"
if not exists (select null from EndpointMessageTypeMetric where MetricId = @MetricId and MessageType = @MessageType)
    insert into EndpointMessageTypeMetric
    (
        MetricId,
        MessageType,
        DateRegistered,
        EndpointId,
        Count,
        TotalExecutionDuration,
        FastestExecutionDuration,
        SlowestExecutionDuration
    )
    values
    (
        @MetricId,
        @MessageType,
        @DateRegistered,
        @Id,
        @Count,
        @TotalExecutionDuration,
        @FastestExecutionDuration,
        @SlowestExecutionDuration
    )
")
                .AddParameterValue(Columns.MetricId, metricId)
                .AddParameterValue(Columns.MessageType, messageType)
                .AddParameterValue(Columns.DateRegistered, dateRegistered)
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.Count, count)
                .AddParameterValue(Columns.TotalExecutionDuration, totalExecutionDuration)
                .AddParameterValue(Columns.FastestExecutionDuration, fastestExecutionDuration)
                .AddParameterValue(Columns.SlowestExecutionDuration, slowestExecutionDuration);
        }

        public IQuery Remove(Guid endpointId)
        {
            return RawQuery.Create(
                    @"delete from Endpoint where Id = @Id")
                .AddParameterValue(Columns.Id, endpointId);
        }

        public IQuery All()
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"order by MachineName"));
        }

        public IQuery Search(string match)
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"
where 
	MachineName like @Match
or
	BaseDirectory like @Match
or
	EntryAssemblyQualifiedName like @Match
or
	InboxWorkQueueUri like @Match
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

        public IQuery AddLogEntry(Guid endpointId, DateTime dateLogged, string message)
        {
            return RawQuery.Create(@"
insert into EndpointLogEntry
(
	EndpointId,
	DateLogged,
	DateRegistered,
	Message
)
values
(
	@Id,
	@DateLogged,
	@DateRegistered,
	@Message
);
")
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(Columns.DateLogged, dateLogged)
                .AddParameterValue(Columns.DateRegistered, DateTime.UtcNow)
                .AddParameterValue(Columns.Message, message);
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
        @DateStopped > DateStopped;
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