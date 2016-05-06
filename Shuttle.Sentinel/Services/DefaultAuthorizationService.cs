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
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class DefaultAuthorizationService : IAuthorizationService, IAnonymousPermissions
	{
		private readonly IConfiguredDatabaseContextFactory _databaseContextFactory;
		private readonly ISystemUserQuery _systemUserQuery;
		private readonly ISystemRoleQuery _systemRoleQuery;

		public DefaultAuthorizationService(IConfiguredDatabaseContextFactory databaseContextFactory, ISystemUserQuery systemUserQuery, ISystemRoleQuery systemRoleQuery)
		{
			Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
			Guard.AgainstNull(systemUserQuery, "systemUserQuery");
			Guard.AgainstNull(systemRoleQuery, "systemRoleQuery");

			_databaseContextFactory = databaseContextFactory;
			_systemUserQuery = systemUserQuery;
			_systemRoleQuery = systemRoleQuery;
		}

		public IEnumerable<string> Permissions(string email, object authenticationTag)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<string> AnonymousPermissions()
		{
			int count;
			var result = new List<string>();

			using (_databaseContextFactory.Create())
			{
				count = _systemUserQuery.Count();
			}

			result.AddRange(_systemRoleQuery.Permissions("Anonymous"));

			if (count == 0)
			{
				result.Add(SystemPermissions.States.UserRequired);
				result.Add(SystemPermissions.Register.User);
			}

			return result;
		}
	}
}