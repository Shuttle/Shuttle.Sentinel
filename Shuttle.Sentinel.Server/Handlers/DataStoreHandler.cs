using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class DataStoreHandler : 
        IMessageHandler<RegisterDataStore>,
        IMessageHandler<RemoveDataStore>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDataStoreQuery _dataStoreQuery;

        public DataStoreHandler(IDatabaseContextFactory databaseContextFactory, IDataStoreQuery dataStoreQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(dataStoreQuery, nameof(dataStoreQuery));

            _databaseContextFactory = databaseContextFactory;
            _dataStoreQuery = dataStoreQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterDataStore> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                _dataStoreQuery.Register(new DataStore
                {
                    Id = !message.Id.HasValue || Guid.Empty.Equals(message.Id) ? Guid.NewGuid() : message.Id.Value,
                    Name = message.Name,
                    ConnectionString = message.ConnectionString,
                    ProviderName = message.ProviderName
                });
            }
        }

        public void ProcessMessage(IHandlerContext<RemoveDataStore> context)
        {
            using (_databaseContextFactory.Create())
            {
                _dataStoreQuery.Remove(context.Message.Id);
            }
        }
    }
}