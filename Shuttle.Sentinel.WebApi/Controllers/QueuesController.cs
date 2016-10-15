using System;
using System.Collections.Generic;
using System.Linq;
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
        public IHttpActionResult Post([FromBody] QueueModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new AddQueueCommand
            {
                QueueUri = model.Uri
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [Route("api/queues/remove")]
        public IHttpActionResult RemoveQueue([FromBody] QueueModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new RemoveQueueCommand
            {
                QueueUri = model.Uri
            });

            return Ok();
        }
    }
}