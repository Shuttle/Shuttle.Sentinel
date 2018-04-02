using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageHeaderQueryFactory : IMessageHeaderQueryFactory
    {
        private const string SelectFrom = @"
select 
    Id, 
    [Key],
    [Value]
from 
    MessageHeader 
";

        public IQuery Add(string key, string value)
        {
            return RawQuery.Create(@"
if not exists(select null from MessageHeader where Key = @Key and Value = @Value) 
    insert into MessageHeader 
    (
        [Key],
        [Value]
    ) 
    values 
    (
        [@Key],
        [@Value]
    )
")
                .AddParameterValue(MessageHeaderColumns.Key, key)
                .AddParameterValue(MessageHeaderColumns.Value, value);
        }

        public IQuery Remove(Guid id)
        {
            return RawQuery.Create(
                    @"delete from MessageHeader where Id = @Id")
                .AddParameterValue(Columns.Id, id);
        }

        public IQuery All()
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"order by Key, Value"));
        }

        public IQuery Search(string match)
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"
where 
    Key like @Match
or
    Value like @Match
order by Uri
"))
                .AddParameterValue(Columns.Match, string.Concat("%", match, "%"));
        }
    }
}