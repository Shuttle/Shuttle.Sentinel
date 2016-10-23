using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class QueueQueryFactory : IQueueQueryFactory
    {
        public IQuery Add(string uri)
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
            return RawQuery.Create(@"select Id, Uri from Queue order by Uri");
        }

        public IQuery Search(string match)
        {
            return RawQuery.Create(@"select Id, Uri from Queue where Uri like @Uri order by Uri")
                .AddParameterValue(QueueColumns.Uri, string.Concat("%", match, "%"));
        }
    }
}