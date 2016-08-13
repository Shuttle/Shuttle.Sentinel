using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
    public class AnonymousPermissionsController : SentinelApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly List<string> _emptyAnonymousPermissions = new List<string>();

        public AnonymousPermissionsController(IAuthorizationService authorizationService)
        {
            Guard.AgainstNull(authorizationService, "authorizationService");

            _authorizationService = authorizationService;
        }

        public IHttpActionResult Get()
        {
            var anonymousPermissions = _authorizationService as IAnonymousPermissions;

            var permissions = anonymousPermissions != null
                ? new List<string>(anonymousPermissions.AnonymousPermissions())
                : _emptyAnonymousPermissions;

            return Ok(new
            {
                IsUserRequired = permissions.Contains(SystemPermissions.Register.UserRequired),
                Permissions =
                    from permission in permissions
                    select new
                    {
                        Permission = permission
                    }
            });
        }
    }
}