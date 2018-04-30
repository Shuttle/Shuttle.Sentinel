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
	MessageTypeMetric
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
    }
}