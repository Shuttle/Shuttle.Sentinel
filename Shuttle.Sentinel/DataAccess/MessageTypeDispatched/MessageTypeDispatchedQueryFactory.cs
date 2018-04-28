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
    MessageType,
    RecipientInboxWorkQueueUri,
    count(*) EndpointCount
from
    MessageTypeDispatched
where 
	MessageType like @Match
or
    RecipientInboxWorkQueueUri like @Match
group by
    MessageType,
    RecipientInboxWorkQueueUri
order by
    MessageType
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}