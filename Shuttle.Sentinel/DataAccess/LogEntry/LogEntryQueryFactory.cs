using Shuttle.Core.Data;
using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.DataAccess.LogEntry
{
    public class LogEntryQueryFactory : ILogEntryQueryFactory
    {

        public IQuery Register(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
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

        public IQuery Search(Query.LogEntry.Specification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return RawQuery.Create($@"
select top {specification.MaximumRows}
	EndpointId,
	DateLogged,
	DateRegistered,
	Message,
	LogLevel,
	Category,
	EventId,
	Scope
from
	EndpointLogEntry
where
(
	@StartDateLogged is null
or
	DateLogged >= @StartDateLogged
)
and
(
	@EndDateLogged is null
or
	DateLogged < @EndDateLogged
)
and
(
	@CategoryMatch is null
or
	Category like '%' + @CategoryMatch + '%'
)
and
(
	@MessageMatch is null
or
	Message like '%' + @MessageMatch + '%'
)
and
(
	@LogLevel is null
or
	LogLevel >= LogLevel
)
")
                .AddParameterValue(Columns.StartDateLogged, specification.StartDateLogged)
                .AddParameterValue(Columns.EndDateLogged, specification.EndDateLogged)
                .AddParameterValue(Columns.CategoryMatch, specification.CategoryMatch)
                .AddParameterValue(Columns.MessageMatch, specification.MessageMatch)
                .AddParameterValue(Columns.LogLevel, specification.LogLevel);
        }
    }
}