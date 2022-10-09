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
	e.EnvironmentName,
    MessageTypeHandled,
    MessageTypeDispatched,
    count(*) EndpointCount
from
    EndpointMessageTypeAssociation mta
inner join
	[Endpoint] e on (e.Id = mta.EndpointId)
where 
(
    isnull(@Match, '') = ''
or
	MessageTypeHandled like @Match
or
    MessageTypeDispatched like @Match
)
group by
    e.EnvironmentName,
    MessageTypeHandled,
    MessageTypeDispatched
order by
    MessageTypeHandled
")
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}