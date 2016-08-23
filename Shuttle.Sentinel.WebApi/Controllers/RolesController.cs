using System;
using System.Linq;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class RolesController : SentinelApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ISessionRepository _sessionRepository;
        private readonly ISystemRoleQuery _systemRoleQuery;

        public RolesController(IDatabaseContextFactory databaseContextFactory, IServiceBus bus, ISessionRepository sessionRepository,
            IAuthorizationService authorizationService, ISystemRoleQuery systemRoleQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(bus, "bus");
            Guard.AgainstNull(sessionRepository, "sessionRepository");
            Guard.AgainstNull(authorizationService, "authorizationService");
            Guard.AgainstNull(systemRoleQuery, "systemRoleQuery");

            _databaseContextFactory = databaseContextFactory;
            _bus = bus;
            _sessionRepository = sessionRepository;
            _authorizationService = authorizationService;
            _systemRoleQuery = systemRoleQuery;
        }

        [RequiresPermission(SystemPermissions.View.Roles)]
        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = from row in _systemRoleQuery.Search()
                        select new
                        {
                            Id = SystemRoleColumns.Id.MapFrom(row),
                            Rolename = SystemRoleColumns.RoleName.MapFrom(row)
                        }
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Roles)]
        public IHttpActionResult Get(Guid id)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = new {} // _systemRoleQuery.Get(id)
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Roles)]
        public IHttpActionResult Post([FromBody] RegisterRoleModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new AddRoleCommand
            {
                Name = model.Name
            });

            return Ok();
        }
    }
}