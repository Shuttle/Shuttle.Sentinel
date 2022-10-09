using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeAssociationQueryFactory : IMessageTypeAssociationQueryFactory
    {
        public IQuery Search(string match)
        {
            return RawQuery.Create(@"
select
    MessageTypeHandled,
    MessageTypeDispatched,
    count(*) EndpointCount
from
    EndpointMessageTypeAssociation
where 
	MessageTypeHandled like @Match
or
    MessageTypeDispatched like @Match
group by
    MessageTypeHandled,
    MessageTypeDispatched
order by
    MessageTypeHandled
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}