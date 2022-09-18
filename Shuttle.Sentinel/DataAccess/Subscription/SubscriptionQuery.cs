using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class SubscriptionQuery : ISubscriptionQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;
        private readonly ISubscriptionQueryFactory _queryFactory;

        public SubscriptionQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, ISubscriptionQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<Subscription> All()
        {
            return _queryMapper.MapObjects<Subscription>(_queryFactory.All());
        }

        public void Add(Subscription subscription)
        {
            _databaseGateway.Execute(_queryFactory.Add(subscription));
        }

        public void Remove(Subscription subscription)
        {
            _databaseGateway.Execute(_queryFactory.Remove(subscription));
        }
    }
}