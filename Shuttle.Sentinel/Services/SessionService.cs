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

using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class SessionService : ISessionService
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IAuthorizationService _authorizationService;
		private readonly ISessionRepository _sessionRepository;

		public SessionService(IAuthenticationService authenticationService,IAuthorizationService authorizationService, ISessionRepository sessionRepository)
		{
			Guard.AgainstNull(authenticationService, "authenticationService");
			Guard.AgainstNull(authorizationService, "authorizationService");
			Guard.AgainstNull(sessionRepository, "sessionRepository");

			_authenticationService = authenticationService;
			_authorizationService = authorizationService;
			_sessionRepository = sessionRepository;
		}

		public RegisterSessionResult Register(string email, string password)
		{
			var authenticationResult = _authenticationService.Authenticate(email,password);

			if (!authenticationResult.Authenticated)
			{
				return RegisterSessionResult.Failure();
			}

			var session = new Session(Guid.NewGuid(), email, DateTime.Now);

			foreach (var permission in _authorizationService.Permissions(email, authenticationResult.AuthenticationTag))
			{
				session.AddPermission(permission);
			}

			_sessionRepository.Save(session);

			return RegisterSessionResult.Success(session.Token, session.Permissions);
		}
	}
}