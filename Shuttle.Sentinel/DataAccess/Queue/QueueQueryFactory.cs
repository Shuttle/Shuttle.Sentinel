using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

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
                .AddParameterValue(Columns.Uri, uri)
                .AddParameterValue(Columns.Processor, processor)
                .AddParameterValue(Columns.Type, type);
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

        public IQuery Search(Queue.Specification specification)
        {
            return RawQuery.Create(string.Concat(SelectFrom, @"
where 
(
    @Id is null
    or
    Id = @Id
)
and
(
    @Uri is null
    or
    Uri like @Uri 
)
order by Uri
"))
                .AddParameterValue(Columns.Id, specification.Id)
                .AddParameterValue(Columns.Uri, string.IsNullOrWhiteSpace(specification.UriMatch) ? null : $"%{specification.UriMatch}%");
        }
    }
}