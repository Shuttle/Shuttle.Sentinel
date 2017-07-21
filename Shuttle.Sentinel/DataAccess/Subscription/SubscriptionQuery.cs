using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class SubscriptionQuery : ISubscriptionQuery
    {
        private readonly IQueryMapper _queryMapper;
        private readonly ISubscriptionQueryFactory _queryFactory;

        public SubscriptionQuery(IQueryMapper queryMapper, ISubscriptionQueryFactory queryFactory)
        {
            Guard.AgainstNull(queryMapper, "queryMapper");
            Guard.AgainstNull(queryFactory, "queryFactory");

            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<Subscription> All()
        {
            return _queryMapper.MapObjects<Subscription>(_queryFactory.All());
        }
    }
}