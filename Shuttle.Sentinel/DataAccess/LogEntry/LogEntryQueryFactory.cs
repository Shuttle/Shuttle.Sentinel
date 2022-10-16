using Shuttle.Core.Data;
using System;
using System.Linq;
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
	le.EndpointId,
	e.EnvironmentName,
	e.MachineName,
	e.BaseDirectory,
	e.EntryAssemblyQualifiedName,
	le.DateLogged,
	le.DateRegistered,
	le.Message,
	le.LogLevel,
	le.Category,
	le.EventId,
	le.Scope
from
	EndpointLogEntry le
inner join
	Endpoint e on (e.Id = le.EndpointId)
where
(
	@StartDateLogged is null
or
	le.DateLogged >= @StartDateLogged
)
and
(
	@EndDateLogged is null
or
	le.DateLogged < @EndDateLogged
)
and
(
	@CategoryMatch is null
or
	le.Category like '%' + @CategoryMatch + '%'
)
and
(
	@MessageMatch is null
or
	le.Message like '%' + @MessageMatch + '%'
)
and
(
	@MachineNameMatch is null
or
	e.MachineName like '%' + @MachineNameMatch + '%'
)
and
(
	@ScopeMatch is null
or
	le.Scope like '%' + @ScopeMatch + '%'
)
{(!specification.LogLevels.Any() ? string.Empty : $@"
and
    le.LogLevel in ({string.Join(",", specification.LogLevels.Select(item => item))})
")}
order by 
	DateLogged desc
")
	            .AddParameterValue(Columns.StartDateLogged, specification.StartDateLogged)
	            .AddParameterValue(Columns.EndDateLogged, specification.EndDateLogged)
	            .AddParameterValue(Columns.CategoryMatch, specification.CategoryMatch)
	            .AddParameterValue(Columns.MessageMatch, specification.MessageMatch)
	            .AddParameterValue(Columns.MachineNameMatch, specification.MachineNameMatch)
	            .AddParameterValue(Columns.ScopeMatch, specification.ScopeMatch);
        }
    }
}