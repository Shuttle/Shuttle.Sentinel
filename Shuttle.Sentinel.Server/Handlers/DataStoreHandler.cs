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
        IMessageHandler<AddDataStoreCommand>,
        IMessageHandler<RemoveDataStoreCommand>
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

        public void ProcessMessage(IHandlerContext<AddDataStoreCommand> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                _dataStoreQuery.Add(new DataStore
                {
                    Id = Guid.NewGuid(),
                    Name = message.Name,
                    ConnectionString = message.ConnectionString,
                    ProviderName = message.ProviderName
                });
            }
        }

        public void ProcessMessage(IHandlerContext<RemoveDataStoreCommand> context)
        {
            using (_databaseContextFactory.Create())
            {
                _dataStoreQuery.Remove(context.Message.Id);
            }
        }
    }
}