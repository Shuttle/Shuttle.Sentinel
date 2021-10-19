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

namespace Shuttle.Sentinel.WebApi
{
    [Route("[controller]")]
    [RequiresPermission(Permissions.Manage.Messages)]
    public class MessageHeadersController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageHeaderQuery _queueQuery;

        public MessageHeadersController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory, IMessageHeaderQuery queueQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(queueQuery, nameof(queueQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _queueQuery = queueQuery;
            _bus = bus;
        }

        [RequiresPermission(Permissions.Manage.Messages)]
        [HttpGet]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _queueQuery.All()
                });
            }
        }

        [RequiresPermission(Permissions.Manage.Messages)]
        [HttpGet("{search}")]
        public IActionResult GetSearch(string search)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _queueQuery.Search(search)
                });
            }
        }

        [RequiresPermission(Permissions.Manage.Messages)]
        [HttpPost]
        public IActionResult Post([FromBody] MessageHeaderModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            var id = Guid.NewGuid();

            _bus.Send(new AddMessageHeaderCommand
            {
                Id = id,
                Key = model.Key,
                Value = model.Value
            });

            return Ok(id);
        }

        [RequiresPermission(Permissions.Manage.Messages)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveMessageHeaderCommand
            {
                Id = id
            });

            return Ok();
        }
    }
}