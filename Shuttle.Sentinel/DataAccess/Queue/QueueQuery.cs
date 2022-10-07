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

        public Guid Save(string uri, string processor, string type)
        {
            return _databaseGateway.GetScalar<Guid>(_queryFactory.Save(uri, processor, type));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.Execute(_queryFactory.Remove(id));
        }

        public IEnumerable<Queue> Search(Queue.Specification specification)
        {
            return _queryMapper.MapObjects<Queue>(_queryFactory.Search(specification));
        }
    }
}