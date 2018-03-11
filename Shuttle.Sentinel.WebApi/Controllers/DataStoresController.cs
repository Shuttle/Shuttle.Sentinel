using System;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class DataStoresController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDataStoreQuery _dataStoreQuery;

        public DataStoresController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IDataStoreQuery dataStoreQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(dataStoreQuery, nameof(dataStoreQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _dataStoreQuery = dataStoreQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _dataStoreQuery.All()
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        public IActionResult Post([FromBody] AddDataStoreModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new AddDataStoreCommand
            {
                Name = model.Name,
                ConnectionString = model.ConnectionString,
                ProviderName = model.ProviderName
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        [Route("api/datastores/{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveDataStoreCommand
            {
                Id = id
            });

            return Ok();
        }
    }
}