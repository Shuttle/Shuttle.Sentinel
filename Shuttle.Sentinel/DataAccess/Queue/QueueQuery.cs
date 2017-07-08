using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class QueueQuery : IQueueQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueueQueryFactory _queueQueryFactory;
        private readonly IQueryMapper _queryMapper;

        public QueueQuery(IDatabaseGateway databaseGateway, IQueueQueryFactory queueQueryFactory, IQueryMapper queryMapper)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(queueQueryFactory, "queueQueryFactory");
            Guard.AgainstNull(queryMapper, "queryMapper");

            _databaseGateway = databaseGateway;
            _queueQueryFactory = queueQueryFactory;
            _queryMapper = queryMapper;
        }

        public void Add(string uri, string displayUri)
        {
            _databaseGateway.ExecuteUsing(_queueQueryFactory.Add(uri, displayUri));
        }

        public void Remove(string uri)
        {
            _databaseGateway.ExecuteUsing(_queueQueryFactory.Remove(uri));
        }

        public IEnumerable<Queue> All()
        {
            return _queryMapper.MapObjects<Queue>(_queueQueryFactory.All());
        }

        public IEnumerable<Queue> Search(string match)
        {
            return _queryMapper.MapObjects<Queue>(_queueQueryFactory.Search(match));
        }
    }
}