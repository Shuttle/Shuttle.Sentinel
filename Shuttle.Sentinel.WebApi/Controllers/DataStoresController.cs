using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class DataStoresController : SentinelApiController
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDataStoreQuery _dataStoreQuery;

        public DataStoresController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IDataStoreQuery dataStoreQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(dataStoreQuery, "dataStoreQuery");
            Guard.AgainstNull(bus, "bus");

            _databaseContextFactory = databaseContextFactory;
            _dataStoreQuery = dataStoreQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        public IHttpActionResult Get()
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
        public IHttpActionResult Post([FromBody] AddDataStoreModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new AddDataStoreCommand
            {
                Name = model.Name,
                ConnectionString = model.ConnectionString,
                ProviderName = model.ProviderName
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        public IHttpActionResult Put([FromBody] EditDataStoreModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new EditDataStoreCommand
            {
                Id = model.Id,
                Name = model.Name,
                ConnectionString = model.ConnectionString,
                ProviderName = model.ProviderName
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.DataStores)]
        [Route("api/dataStores/remove")]
        public IHttpActionResult RemoveDataStore([FromBody] RemoveDataStoreModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new RemoveDataStoreCommand
            {
                Name = model.Name
            });

            return Ok();
        }
    }
}