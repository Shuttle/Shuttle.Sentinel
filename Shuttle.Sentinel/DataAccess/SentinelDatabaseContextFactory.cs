using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class SentinelDatabaseContextFactory : DatabaseContextFactory, ISentinelDatabaseContextFactory
    {
        private readonly IDataStoreQuery _dataStoreQuery;

        public SentinelDatabaseContextFactory(IDbConnectionFactory dbConnectionFactory,
            IDbCommandFactory dbCommandFactory, IDatabaseContextCache databaseContextCache,
            IDataStoreQuery dataStoreQuery)
            : base(dbConnectionFactory, dbCommandFactory, databaseContextCache)
        {
            Guard.AgainstNull(dataStoreQuery, "dataStoreQuery");

            _dataStoreQuery = dataStoreQuery;
        }

        public IDatabaseContext Create(Guid dataStoreId)
        {
            DataStore store;

            using (Create())
            {
                store = _dataStoreQuery.Get(dataStoreId);
            }

            return Create(store.ConnectionString, store.ConnectionString);
        }
    }
}