using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class DataStoreQuery : IDataStoreQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IDataStoreQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public DataStoreQuery(IDatabaseGateway databaseGateway, IDataStoreQueryFactory queryFactory, IQueryMapper queryMapper)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
            _queryMapper = queryMapper;
        }

        public void Add(DataStore dataStore)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Add(dataStore));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Remove(id));
        }

        public IEnumerable<DataStore> Search(DataStore.Specification specification)
        {
            return _queryMapper.MapObjects<DataStore>(_queryFactory.Search(specification));
        }

        public DataStore Get(Guid id)
        {
            var result = _queryMapper.MapObject<DataStore>(_queryFactory.Get(id));

            if (result == null)
            {
                throw new SentinelException($"Could not find a data store with id '{id}'.");
            }

            return result;
        }
    }
}