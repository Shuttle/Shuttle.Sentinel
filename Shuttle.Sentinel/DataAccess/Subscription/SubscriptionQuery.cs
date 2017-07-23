using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class SubscriptionQuery : ISubscriptionQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;
        private readonly ISubscriptionQueryFactory _queryFactory;

        public SubscriptionQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, ISubscriptionQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(queryMapper, "queryMapper");
            Guard.AgainstNull(queryFactory, "queryFactory");

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
            _databaseGateway.ExecuteUsing(_queryFactory.Add(subscription));
        }

        public void Remove(Subscription subscription)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Remove(subscription));
        }
    }
}