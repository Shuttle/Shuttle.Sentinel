/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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