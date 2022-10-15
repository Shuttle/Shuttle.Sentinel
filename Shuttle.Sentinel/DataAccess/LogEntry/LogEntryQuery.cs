using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.DataAccess.LogEntry
{
    public class LogEntryQuery : ILogEntryQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ILogEntryQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public LogEntryQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            ILogEntryQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<Query.LogEntry> Search(Query.LogEntry.Specification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return _queryMapper.MapObjects<Query.LogEntry>(_queryFactory.Search(specification));
        }

        public void Register(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
            int eventId, string scope)
        {
            _databaseGateway.Execute(_queryFactory.Register(endpointId, dateLogged, message, logLevel, category, eventId, scope));
        }
    }
}