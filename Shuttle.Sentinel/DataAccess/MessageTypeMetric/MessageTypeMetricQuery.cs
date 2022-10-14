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
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeMetricQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, IMessageTypeMetricQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeMetric> Search(DateTime @from, string match)
        {
            return _queryMapper.MapObjects<MessageTypeMetric>(_queryFactory.Search(@from, match));
        }

        public void Register(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count,
            double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
            _databaseGateway.Execute(_queryFactory.Register(metricId, messageType, dateRegistered,
                endpointId,
                count, fastestExecutionDuration, slowestExecutionDuration, totalExecutionDuration));
        }
    }
}