using System;
using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class DataStoreQuery : IDataStoreQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IDataStoreQueryFactory _dataStoreQueryFactory;
        private readonly IQueryMapper _queryMapper;

        public DataStoreQuery(IDatabaseGateway databaseGateway, IDataStoreQueryFactory dataStoreQueryFactory, IQueryMapper queryMapper)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(dataStoreQueryFactory, "dataStoreQueryFactory");
            Guard.AgainstNull(queryMapper, "queryMapper");

            _databaseGateway = databaseGateway;
            _dataStoreQueryFactory = dataStoreQueryFactory;
            _queryMapper = queryMapper;
        }

        public void Add(DataStore dataStore)
        {
            _databaseGateway.ExecuteUsing(_dataStoreQueryFactory.Add(dataStore));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.ExecuteUsing(_dataStoreQueryFactory.Remove(id));
        }

        public IEnumerable<DataStore> All()
        {
            return _queryMapper.MapObjects<DataStore>(_dataStoreQueryFactory.All());
        }

        public DataStore Get(Guid id)
        {
            var result = _queryMapper.MapObject<DataStore>(_dataStoreQueryFactory.Get(id));

            if (result == null)
            {
                throw new SentinelException($"Could not find a data store with id '{id}'.");
            }

            return result;
        }
    }
}