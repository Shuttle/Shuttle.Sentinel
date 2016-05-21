using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
	public class AnonymousPermissionsController : ApiController
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
				? anonymousPermissions.AnonymousPermissions()
				: _emptyAnonymousPermissions;

			return Ok(new
			{
				Data =
					from permission in permissions
					select new
					{
						Permission = permission
					}
			});
		}
	}
}