using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
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

        [RequiresPermission(Permissions.Manage.DataStores)]
        [HttpGet("{id?}")]
        public IActionResult Get(Guid id)
        {
            var specification = new DataStore.Specification();

            if (!Guid.Empty.Equals(id))
            {
                specification.WithId(id);
            }

            using (_databaseContextFactory.Create())
            {
                var result = _dataStoreQuery.Search(specification);

                try
                {
                    return Ok(Guid.Empty.Equals(id) ? result : result.FirstOrDefault().GuardAgainstRecordNotFound(id));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [RequiresPermission(Permissions.Manage.DataStores)]
        [HttpPost]
        public IActionResult Post([FromBody] RegisterDataStore command)
        {
            Guard.AgainstNull(command, nameof(command));

            _bus.Send(command);

            return Ok();
        }

        [RequiresPermission(Permissions.Manage.DataStores)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveDataStore
            {
                Id = id
            });

            return Ok();
        }
    }
}