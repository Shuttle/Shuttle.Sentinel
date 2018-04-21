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
        private readonly IQueueQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public QueueQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, IQueueQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
            _queryMapper = queryMapper;
        }

        public void Save(string uri, string processor, string type)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Save(uri, processor, type));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Remove(id));
        }

        public IEnumerable<Queue> All()
        {
            return _queryMapper.MapObjects<Queue>(_queryFactory.All());
        }

        public IEnumerable<Queue> Search(string match)
        {
            return _queryMapper.MapObjects<Queue>(_queryFactory.Search(match));
        }
    }
}