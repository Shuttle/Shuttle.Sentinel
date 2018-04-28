using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeHandledQueryFactory : IMessageTypeHandledQueryFactory
    {
        public IQuery Search(string match)
        {
            return RawQuery.Create(@"
select
    MessageType,
    count(*) EndpointCount
from
    MessageTypeHandled
where 
	MessageType like @Match
group by
    MessageType
order by
    MessageType
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}