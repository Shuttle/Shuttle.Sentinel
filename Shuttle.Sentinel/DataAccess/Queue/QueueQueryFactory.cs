using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class QueueQueryFactory : IQueueQueryFactory
    {
        private const string SelectFrom = @"
select 
    Id, 
    Uri,
    Processor,
    Type
from 
    Queue 
";

        public IQuery Save(string uri, string processor, string type)
        {
            return RawQuery.Create(@"
if exists(select null from Queue where Uri = @Uri)
    update
        Queue
    set
        Processor = @Processor,
        Type = @Type
    where
        Uri = @Uri
else
    insert into Queue 
    (
        Uri,
        Processor,
        Type
    ) 
    values 
    (
        @Uri,
        @Processor,
        @Type
    )
")
                .AddParameterValue(QueueColumns.Uri, uri)
                .AddParameterValue(QueueColumns.Processor, processor)
                .AddParameterValue(QueueColumns.Type, type);
        }

        public IQuery Remove(Guid id)
        {
            return RawQuery.Create(
                    @"delete from Queue where Id = @Id")
                .AddParameterValue(Columns.Id, id);
        }

        public IQuery All()
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"order by Uri"));
        }

        public IQuery Search(string match)
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"
where 
    Uri like @Uri 
order by Uri
"))
                .AddParameterValue(QueueColumns.Uri, string.Concat("%", match, "%"));
        }
    }
}