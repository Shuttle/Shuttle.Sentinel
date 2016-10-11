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

        public QueuesController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(bus, "bus");

            _databaseContextFactory = databaseContextFactory;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Queues)]
        public IHttpActionResult Get()
        {
            var queues = new List<string>
            {
                "rabbitmq://shuttle:shuttle!@localhost/shuttle-server-work",
                "rabbitmq://shuttle:shuttle!@localhost/shuttle-error"
            };

            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Queues = queues
                    //Data = from row in _systemRoleQuery.Search()
                    //    select new
                    //    {
                    //        Id = SystemRoleColumns.Id.MapFrom(row),
                    //        RoleName = SystemRoleColumns.RoleName.MapFrom(row)
                    //    }
                });
            }
        }
    }
}