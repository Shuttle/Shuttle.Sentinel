using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class DataStoreDatabaseContextFactory : DatabaseContextFactory, IDataStoreDatabaseContextFactory
    {
        private readonly IDataStoreQuery _dataStoreQuery;

        public DataStoreDatabaseContextFactory(
            IConnectionConfigurationProvider connectionConfigurationProvider,
            IDbConnectionFactory dbConnectionFactory,
            IDbCommandFactory dbCommandFactory, 
            IDatabaseContextCache databaseContextCache,
            IDataStoreQuery dataStoreQuery)
            : base(
                connectionConfigurationProvider,
                dbConnectionFactory, 
                dbCommandFactory, 
                databaseContextCache)
        {
            Guard.AgainstNull(dataStoreQuery, nameof(dataStoreQuery));

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