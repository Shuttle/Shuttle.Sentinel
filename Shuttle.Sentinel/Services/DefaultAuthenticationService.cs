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
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall;

namespace Shuttle.Sentinel
{
	public class DefaultAuthenticationService : IAuthenticationService
	{
		private readonly IConfiguredDatabaseContextFactory _databaseContextFactory;
		private readonly IEventStore _eventStore;
		private readonly IKeyStore _keyStore;
		private readonly IHashingService _hashingService;
		private readonly ILog _log;

		public DefaultAuthenticationService(IConfiguredDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore, IHashingService hashingService)
		{
			Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
			Guard.AgainstNull(eventStore, "eventStore");
			Guard.AgainstNull(keyStore, "keyStore");
			Guard.AgainstNull(hashingService, "hashingService");

			_databaseContextFactory = databaseContextFactory;
			_eventStore = eventStore;
			_keyStore = keyStore;
			_hashingService = hashingService;

			_log = Log.For(this);
		}

		public AuthenticationResult Authenticate(string username, string password)
		{
			EventStream stream;
			Guid? userId;

			using (_databaseContextFactory.Create())
			{
				userId = _keyStore.Get(User.Key(username));

				if (!userId.HasValue)
				{
					_log.Trace(string.Format("[username not found] : username = '{0}'", username));

					return AuthenticationResult.Failure();
				}

				stream = _eventStore.Get(userId.Value);
			}

			var user = new User(userId.Value);

			stream.Apply(user);

			if (!user.Active)
			{
				return AuthenticationResult.Failure(AuthenticationResultType.ActivationRequired);
			}

			if (user.PasswordMatches(_hashingService.Sha256(password)))
			{
				return AuthenticationResult.Success();
			}

			_log.Trace(string.Format("[invalid password] : username = '{0}'", username));

			return AuthenticationResult.Failure();
		}
	}
}