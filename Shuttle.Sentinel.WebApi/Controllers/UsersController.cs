using System;
using System.Linq;
using System.Threading;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class UsersController : SentinelApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IHashingService _hashingService;
        private readonly ISessionRepository _sessionRepository;
        private readonly ISystemUserQuery _systemUserQuery;

        public UsersController(IDatabaseContextFactory databaseContextFactory, IServiceBus bus,
            IHashingService hashingService, ISessionRepository sessionRepository,
            IAuthorizationService authorizationService, ISystemUserQuery systemUserQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(bus, "bus");
            Guard.AgainstNull(hashingService, "hashingService");
            Guard.AgainstNull(sessionRepository, "sessionRepository");
            Guard.AgainstNull(authorizationService, "authorizationService");
            Guard.AgainstNull(systemUserQuery, "systemUserQuery");

            _databaseContextFactory = databaseContextFactory;
            _bus = bus;
            _hashingService = hashingService;
            _sessionRepository = sessionRepository;
            _authorizationService = authorizationService;
            _systemUserQuery = systemUserQuery;
        }

        [RequiresPermission(SystemPermissions.View.Users)]
        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = from row in _systemUserQuery.Search()
                        select new
                        {
                            Id = SystemUserColumns.Id.MapFrom(row),
                            Username = SystemUserColumns.Username.MapFrom(row),
                            DateRegistered = SystemUserColumns.DateRegistered.MapFrom(row),
                            RegisteredBy = SystemUserColumns.RegisteredBy.MapFrom(row)
                        }
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Users)]
        public IHttpActionResult Get(Guid id)
        {
             using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _systemUserQuery.Get(id)
                });
            }
        }

        public IHttpActionResult Post([FromBody] RegisterUserModel model)
        {
            Guard.AgainstNull(model, "model");

            var registeredBy = "system";
            var result = GetSessionToken();
            var ok = false;

            if (result.OK)
            {
                using (_databaseContextFactory.Create())
                {
                    var session = _sessionRepository.Get(result.SessionToken);

                    registeredBy = session.Username;

                    ok = session.HasPermission(SystemPermissions.Register.User);
                }
            }
            else
            {
                int count;

                using (_databaseContextFactory.Create())
                {
                    count = _systemUserQuery.Count();
                }

                if (count == 0)
                {
                    var anonymousPermissions = _authorizationService as IAnonymousPermissions;

                    if (anonymousPermissions != null)
                    {
                        ok = anonymousPermissions.HasPermission(SystemPermissions.Register.User);
                    }
                }
            }

            if (!ok)
            {
                return Unauthorized();
            }

            _bus.Send(new RegisterUserCommand
            {
                Username = model.Username,
                PasswordHash = _hashingService.Sha256(model.Password),
                RegisteredBy = registeredBy
            });

            return Ok();
        }
    }
}