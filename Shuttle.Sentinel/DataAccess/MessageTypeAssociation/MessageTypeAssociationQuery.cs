using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeAssociationQuery : IMessageTypeAssociationQuery
    {
        private readonly IMessageTypeAssociationQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeAssociationQuery(IQueryMapper queryMapper,
            IMessageTypeAssociationQueryFactory queryFactory)
        {
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeAssociation> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeAssociation>(_queryFactory.Search(match));
        }
    }
}