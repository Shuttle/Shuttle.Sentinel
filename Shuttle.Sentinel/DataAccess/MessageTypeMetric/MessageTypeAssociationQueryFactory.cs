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
	Sum(Count) Count,
	Sum(TotalExecutionDuration) TotalExecutionDuration,
	Min(FastestExecutionDuration) FastestExecutionDuration,
	Max(SlowestExecutionDuration) SlowestExecutionDuration,
	Sum(TotalExecutionDuration) / Sum(Count) AverageExecutionDuration
from
	MessageTypeMetric
where
	DateRegistered > @Date
and
	MessageType = @Match
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