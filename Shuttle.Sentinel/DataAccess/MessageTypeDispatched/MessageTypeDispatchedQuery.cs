using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeDispatchedQuery : IMessageTypeDispatchedQuery
    {
        private readonly IMessageTypeDispatchedQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeDispatchedQuery(IQueryMapper queryMapper,
            IMessageTypeDispatchedQueryFactory queryFactory)
        {
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeDispatched> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeDispatched>(_queryFactory.Search(match));
        }
    }
}