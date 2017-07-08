using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class QueueQueryFactory : IQueueQueryFactory
    {
        private const string SelectFrom = @"
select 
    Id, 
    Uri, 
    DisplayUri 
from 
    Queue 
";

        public IQuery Add(string uri, string displayUri)
        {
            return RawQuery.Create(
                @"if not exists(select null from Queue where Uri = @Uri) insert into Queue (Uri) values (@Uri)")
                .AddParameterValue(QueueColumns.Uri, uri);
        }

        public IQuery Remove(string uri)
        {
            return RawQuery.Create(
                @"delete from Queue where Uri = @Uri")
                .AddParameterValue(QueueColumns.Uri, uri);
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