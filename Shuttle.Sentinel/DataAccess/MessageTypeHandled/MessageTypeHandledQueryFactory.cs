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
    e.EnvironmentName,
    MessageType,
    count(*) EndpointCount
from
    EndpointMessageTypeHandled mta
inner join
	[Endpoint] e on (e.Id = mta.EndpointId)
where 
(
    isnull(@Match, '') = ''
or
	MessageType like @Match
)
group by
    e.EnvironmentName,
    MessageType
order by
    MessageType
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}