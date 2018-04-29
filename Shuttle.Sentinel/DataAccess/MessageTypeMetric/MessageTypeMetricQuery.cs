using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeMetricQuery : IMessageTypeMetricQuery
    {
        private readonly IMessageTypeMetricQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeMetricQuery(IQueryMapper queryMapper,
            IMessageTypeMetricQueryFactory queryFactory)
        {
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeMetric> Search(DateTime @from, string match)
        {
            return _queryMapper.MapObjects<MessageTypeMetric>(_queryFactory.Search(@from, match));
        }
    }
}