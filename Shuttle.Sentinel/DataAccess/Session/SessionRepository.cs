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
using System.Data;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class SessionRepository : ISessionRepository
	{
		private readonly IDatabaseGateway _databaseGateway;
		private readonly ISessionQueryFactory _queryFactory;
		private readonly IDataRepository<Session> _dataRepository;

		public SessionRepository(IDatabaseGateway databaseGateway, IDataRepository<Session> dataRepository, ISessionQueryFactory queryFactory)
		{
			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(dataRepository, "dataRepository");
			Guard.AgainstNull(queryFactory, "queryFactory");

			_databaseGateway = databaseGateway;
			_dataRepository = dataRepository;
			_queryFactory = queryFactory;
		}

		public void Save(Session session)
		{
			Guard.AgainstNull(session, "session");

			_databaseGateway.ExecuteUsing(_queryFactory.Remove(session.EMail));
			_databaseGateway.ExecuteUsing(_queryFactory.Add(session));

			foreach (var permission in session.Permissions)
			{
				_databaseGateway.ExecuteUsing(_queryFactory.AddPermission(session.Token, permission));
			}
		}

		public Session Get(Guid token)
		{
			var session = _dataRepository.FetchItemUsing(_queryFactory.Get(token));

			foreach (var row in _databaseGateway.GetRowsUsing(_queryFactory.GetPermissions(token)))
			{
				session.AddPermission(SessionPermissionColumns.Permission.MapFrom(row));
			}

			return session;
		}

		public void Remove(Guid token)
		{
			_databaseGateway.ExecuteUsing(_queryFactory.Remove(token));
		}
	}
}