using System;
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
    OutboxErrorQueueUri
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
                .AddParameterValue(EndpointColumns.EntryAssemblyQualifiedName, entryAssemblyQualifiedName)
                .AddParameterValue(EndpointColumns.IPv4Address, ipv4Address)
                .AddParameterValue(EndpointColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.InboxErrorQueueUri, inboxErrorQueueUri)
                .AddParameterValue(EndpointColumns.InboxDeferredQueueUri, inboxDeferredQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxWorkQueueUri, controlInboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.ControlInboxErrorQueueUri, controlInboxErrorQueueUri)
                .AddParameterValue(EndpointColumns.OutboxWorkQueueUri, outboxWorkQueueUri)
                .AddParameterValue(EndpointColumns.OutboxErrorQueueUri, outboxErrorQueueUri);
        }

        public IQuery AddMessageTypeHandled(Guid endpointId, string messageType)
        {
            return RawQuery.Create(@"
if not exists (select null from MessageTypeHandled where EndpointId = @Id and MessageType = @MessageType)
    insert into MessageTypeHandled
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
                .AddParameterValue(MessageColumns.MessageType, messageType);
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
        MessageTypeDispatched
    where
        EndpointId = @Id
    and
        MessageType = @MessageType
    and
        RecipientInboxWorkQueueUri = @RecipientInboxWorkQueueUri 
)
    insert into MessageTypeDispatched
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
                .AddParameterValue(MessageColumns.MessageType, dispatchedMessageType)
                .AddParameterValue(MessageColumns.RecipientInboxWorkQueueUri, recipientInboxWorkQueueUri);
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
        MessageTypeAssociation
    where
        EndpointId = @Id
    and
        MessageTypeHandled = @MessageTypeHandled
    and
        MessageTypeDispatched = @MessageTypeDispatched
)
    insert into MessageTypeAssociation
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
                .AddParameterValue(MessageColumns.MessageTypeHandled, messageTypeHandled)
                .AddParameterValue(MessageColumns.MessageTypeDispatched, messageTypeDispatched);
        }

        public IQuery AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
            return RawQuery.Create(@"
if not exists (select null from MessageTypeMetric where MetricId = @MetricId and MessageType = @MessageType)
    insert into MessageTypeMetric
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
                .AddParameterValue(MessageColumns.MetricId, metricId)
                .AddParameterValue(MessageColumns.MessageType, messageType)
                .AddParameterValue(MessageColumns.DateRegistered, dateRegistered)
                .AddParameterValue(Columns.Id, endpointId)
                .AddParameterValue(MessageColumns.Count, count)
                .AddParameterValue(MessageColumns.TotalExecutionDuration, totalExecutionDuration)
                .AddParameterValue(MessageColumns.FastestExecutionDuration, fastestExecutionDuration)
                .AddParameterValue(MessageColumns.SlowestExecutionDuration, slowestExecutionDuration);
        }

        public IQuery Remove(Guid id)
        {
            return RawQuery.Create(
                    @"delete from Endpoint where Id = @Id")
                .AddParameterValue(Columns.Id, id);
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
    }
}