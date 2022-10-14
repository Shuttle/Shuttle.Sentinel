using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeAssociationQuery : IMessageTypeAssociationQuery
    {
        private readonly IMessageTypeAssociationQueryFactory _queryFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeAssociationQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            IMessageTypeAssociationQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeAssociation> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeAssociation>(_queryFactory.Search(match));
        }

        public void Register(Guid endpointId, string messageTypeHandled, string messageTypeDispatched)
        {
            _databaseGateway.Execute(_queryFactory.Register(endpointId, messageTypeHandled, messageTypeDispatched));
        }
    }
}