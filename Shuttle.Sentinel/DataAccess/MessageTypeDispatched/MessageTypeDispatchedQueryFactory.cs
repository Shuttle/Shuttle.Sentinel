using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeDispatchedQueryFactory : IMessageTypeDispatchedQueryFactory
    {
        public IQuery Search(string match)
        {
            return RawQuery.Create(@"
select
    e.EnvironmentName,
    MessageType,
    RecipientInboxWorkQueueUri,
    count(*) EndpointCount
from
    EndpointMessageTypeDispatched mtd
inner join
	[Endpoint] e on (e.Id = mtd.EndpointId)
where 
(
    isnull(@Match, '') = ''
or
	MessageType like @Match
or
    RecipientInboxWorkQueueUri like @Match
)
group by
    e.EnvironmentName,
    MessageType,
    RecipientInboxWorkQueueUri
order by
    MessageType
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}