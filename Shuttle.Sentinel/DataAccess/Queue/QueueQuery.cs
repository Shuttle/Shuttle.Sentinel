using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class QueueQuery : IQueueQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueueQueryFactory _queueQueryFactory;
        private readonly IQueryMapper _queryMapper;

        public QueueQuery(IDatabaseGateway databaseGateway, IQueueQueryFactory queueQueryFactory, IQueryMapper queryMapper)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queueQueryFactory, nameof(queueQueryFactory));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));

            _databaseGateway = databaseGateway;
            _queueQueryFactory = queueQueryFactory;
            _queryMapper = queryMapper;
        }

        public void Add(string uri, string displayUri)
        {
            _databaseGateway.ExecuteUsing(_queueQueryFactory.Add(uri, displayUri));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.ExecuteUsing(_queueQueryFactory.Remove(id));
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