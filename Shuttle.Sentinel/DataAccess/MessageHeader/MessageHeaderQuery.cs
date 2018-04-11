using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageHeaderQuery : IMessageHeaderQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IMessageHeaderQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public MessageHeaderQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper, IMessageHeaderQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
            _queryMapper = queryMapper;
        }

        public void Save(string uri, string displayUri)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Save(uri, displayUri));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Remove(id));
        }

        public IEnumerable<MessageHeader> All()
        {
            return _queryMapper.MapObjects<MessageHeader>(_queryFactory.All());
        }

        public IEnumerable<MessageHeader> Search(string match)
        {
            return _queryMapper.MapObjects<MessageHeader>(_queryFactory.Search(match));
        }
    }
}