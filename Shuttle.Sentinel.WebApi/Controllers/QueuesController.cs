using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    [Route("api/[controller]")]
    public class QueuesController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IQueueQuery _queueQuery;

        public QueuesController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory, IQueueQuery queueQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(queueQuery, nameof(queueQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _queueQuery = queueQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [HttpGet]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = Data(_queueQuery.All())
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [HttpGet("{search}")]
        public IActionResult GetSearch(string search)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = Data(_queueQuery.Search(search))
                });
            }
        }

        private IEnumerable<dynamic> Data(IEnumerable<Queue> queues)
        {
            var result = new List<dynamic>();

            foreach (var queue in queues)
            {
                string securedUri;

                try
                {
                    securedUri = new Uri(queue.Uri).Secured().ToString();
                }
                catch
                {
                    securedUri = "(invalid uri)";
                }

                result.Add(new
                {
                    queue.Id,
                    queue.Uri,
                    queue.Processor,
                    queue.Type,
                    SecuredUri = securedUri
                });
            }

            return result;
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [HttpPost]
        public IActionResult Post([FromBody] QueueModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            try
            {
                var uri = new Uri(model.Uri);
            }
            catch (Exception)
            {
                return BadRequest(string.Format(Resources.InvalidUri, model.Uri));
            }

            _bus.Send(new SaveQueueCommand
            {
                QueueUri = model.Uri,
                Processor = model.Processor,
                Type = model.Type
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveQueueCommand
            {
                Id = id
            });

            return Ok();
        }
    }
}