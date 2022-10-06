using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.DataAccess.Tag
{
    public class TagQuery : ITagQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ITagQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public TagQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            ITagQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public void Register(Guid ownerId, string tag)
        {
            _databaseGateway.Execute(_queryFactory.Register(ownerId, tag));
        }

        public void Remove(Guid ownerId, string tag)
        {
            _databaseGateway.Execute(_queryFactory.Remove(ownerId, tag));
        }

        public IEnumerable<string> Find(Guid ownerId)
        {
            return _queryMapper.MapValues<string>(_queryFactory.Find(ownerId));
        }
    }
}