using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageTypeHandledQuery : IMessageTypeHandledQuery
    {
        private readonly IMessageTypesHandledQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public MessageTypeHandledQuery(IQueryMapper queryMapper,
            IMessageTypesHandledQueryFactory queryFactory)
        {
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<MessageTypeHandled> Search(string match)
        {
            return _queryMapper.MapObjects<MessageTypeHandled>(_queryFactory.Search(match));
        }
    }
}