using System;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class QueuesController : SentinelApiController
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IQueueQuery _queueQuery;

        public QueuesController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory, IQueueQuery queueQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(queueQuery, "queueQuery");
            Guard.AgainstNull(bus, "bus");

            _databaseContextFactory = databaseContextFactory;
            _queueQuery = queueQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _queueQuery.All()
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [Route("api/queues/{search}")]
        public IHttpActionResult GetSearch(string search)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _queueQuery.Search(search)
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        public IHttpActionResult Post([FromBody] QueueModel model)
        {
            Guard.AgainstNull(model, "model");

            try
            {
                var uri = new Uri(model.Uri);
            }
            catch (Exception)
            {
                return BadRequest(string.Format(SentinelResources.InvalidUri, model.Uri));
            }

            _bus.Send(new AddQueueCommand
            {
                QueueUri = model.Uri
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [Route("api/queues/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveQueueCommand
            {
                Id = id
            });

            return Ok();
        }
    }
}