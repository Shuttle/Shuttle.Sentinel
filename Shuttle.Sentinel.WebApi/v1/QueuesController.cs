using System;
using System.Collections.Generic;
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

        [RequiresPermission(Permissions.Manage.Queues)]
        [HttpGet("{id?}")]
        public IActionResult Get(Guid id)
        {
            var specification = new Queue.Specification();

            if (!Guid.Empty.Equals(id))
            {
                specification.WithId(id);
            }

            using (_databaseContextFactory.Create())
            {
                var result = Data(_queueQuery.Search(specification));

                try
                {
                    return Ok(Guid.Empty.Equals(id) ? result : ((object)result.FirstOrDefault()).GuardAgainstRecordNotFound(id));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [RequiresPermission(Permissions.Manage.Queues)]
        [HttpGet("uri")]
        public IActionResult MatchingUri(string match)
        {
            if (string.IsNullOrWhiteSpace(match))
            {
                return BadRequest();
            }

            using (_databaseContextFactory.Create())
            {
                return Ok(Data(_queueQuery.Search(new Queue.Specification().MatchingUri(match))));
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
                    securedUri = new Uri(queue.Uri).ToString();
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

        [RequiresPermission(Permissions.Manage.Queues)]
        [HttpPost]
        public IActionResult Post([FromBody] RegisterQueue command)
        {
            Guard.AgainstNull(command, nameof(command));

            try
            {
                _ = new Uri(command.Uri);
            }
            catch (Exception)
            {
                return BadRequest(string.Format(Resources.InvalidUri, command.Uri));
            }

            _bus.Send(command);

            return Ok();
        }

        [RequiresPermission(Permissions.Manage.Queues)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveQueue
            {
                Id = id
            });

            return Ok();
        }
    }
}