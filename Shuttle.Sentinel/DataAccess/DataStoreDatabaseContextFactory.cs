using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class DataStoreDatabaseContextFactory : DatabaseContextFactory, IDataStoreDatabaseContextFactory
    {
        private readonly IDataStoreQuery _dataStoreQuery;

        public DataStoreDatabaseContextFactory(IDbConnectionFactory dbConnectionFactory,
            IDbCommandFactory dbCommandFactory, IDatabaseContextCache databaseContextCache,
            IDataStoreQuery dataStoreQuery)
            : base(dbConnectionFactory, dbCommandFactory, databaseContextCache)
        {
            Guard.AgainstNull(dataStoreQuery, "dataStoreQuery");

            _dataStoreQuery = dataStoreQuery;
        }

        public IDatabaseContext Create(Guid dataStoreId)
        {
            DataStore dataStore;

            using (Create())
            {
                dataStore = _dataStoreQuery.Get(dataStoreId);
            }

            if (dataStore == null)
            {
                throw new InvalidOperationException($"No data store could be retrieved that has an id of '{dataStoreId}'.");
            }

            return Create(dataStore.ProviderName, dataStore.ConnectionString);
        }
    }
}