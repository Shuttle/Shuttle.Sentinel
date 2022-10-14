using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeMetricQueryFactory : IMessageTypeMetricQueryFactory
    {
        public IQuery Search(DateTime @from, string match)
        {
            return RawQuery.Create(@"
select
	MessageType,
	sum(Count) Count,
	round(sum(TotalExecutionDuration), 3) TotalExecutionDuration,
	round(min(FastestExecutionDuration), 3) FastestExecutionDuration,
	round(max(SlowestExecutionDuration), 3) SlowestExecutionDuration,
	round(sum(TotalExecutionDuration) / sum(Count), 3) AverageExecutionDuration
from
	EndpointMessageTypeMetric
where
	DateRegistered > @Date
and
	MessageType like @Match
group by
	MessageType
order by
	AverageExecutionDuration desc
")
                .AddParameterValue(Columns.Date, from)
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
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