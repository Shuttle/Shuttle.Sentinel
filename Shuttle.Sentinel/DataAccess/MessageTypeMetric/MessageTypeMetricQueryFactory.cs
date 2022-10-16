using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeMetricQueryFactory : IMessageTypeMetricQueryFactory
    {
        public IQuery Search(MessageTypeMetric.Specification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return RawQuery.Create(@"
select
	mtm.MessageType,
	e.EnvironmentName,
	sum(mtm.Count) Count,
	round(sum(mtm.TotalExecutionDuration), 3) TotalExecutionDuration,
	round(min(mtm.FastestExecutionDuration), 3) FastestExecutionDuration,
	round(max(mtm.SlowestExecutionDuration), 3) SlowestExecutionDuration,
	round(sum(mtm.TotalExecutionDuration) / sum(Count), 3) AverageExecutionDuration
from
	EndpointMessageTypeMetric mtm
inner join
	Endpoint e on e.Id = mtm.EndpointId
where
	mtm.DateRegistered > @StartDateRegistered
and
(
	@MessageTypeMatch is null
or
	mtm.MessageType like '%' + @MessageTypeMatch + '%'
)
group by
	mtm.MessageType,
	e.EnvironmentName
order by
	mtm.MessageType,
	e.EnvironmentName
")
                .AddParameterValue(Columns.StartDateRegistered, specification.StartDateRegistered)
                .AddParameterValue(Columns.MessageTypeMatch, specification.MessageTypeMatch);
        }

        public IQuery Register(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
	        int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
	        return RawQuery.Create(@"
if not exists 
(
	select 
		null 
	from 
		EndpointMessageTypeMetric 
	where 
		MetricId = @MetricId 
	and 
		MessageType = @MessageType
)
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
    }
}