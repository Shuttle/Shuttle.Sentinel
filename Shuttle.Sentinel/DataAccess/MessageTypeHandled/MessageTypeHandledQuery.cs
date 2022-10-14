using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeHandledQuery : IMessageTypeHandledQuery
    {
        private readonly IMessageTypeHandledQueryFactory _queryFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeHandledQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, IMessageTypeHandledQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeHandled> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeHandled>(_queryFactory.Search(match));
        }

        public void Register(Guid endpointId, string messageType)
        {
            _databaseGateway.Execute(
                _queryFactory.Register(endpointId, messageType));
        }
    }
}