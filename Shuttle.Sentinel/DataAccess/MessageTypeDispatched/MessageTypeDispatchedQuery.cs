using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeDispatchedQuery : IMessageTypeDispatchedQuery
    {
        private readonly IMessageTypeDispatchedQueryFactory _queryFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeDispatchedQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            IMessageTypeDispatchedQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeDispatched> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeDispatched>(_queryFactory.Search(match));
        }
        
        public void Register(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri)
        {
            _databaseGateway.Execute(
                _queryFactory.Register(endpointId, dispatchedMessageType, recipientInboxWorkQueueUri));
        }
    }
}