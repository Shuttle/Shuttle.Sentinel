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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class Session
	{
		private readonly List<string> _permissions = new List<string>();

		public Session(Guid token, string email, DateTime dateRegistered)
		{
			Token = token;
			EMail = email;
			DateRegistered = dateRegistered;
		}

		public Guid Token { get; private set; }
		public string EMail { get; private set; }
		public DateTime DateRegistered { get; set; }

		public IEnumerable<string> Permissions => new ReadOnlyCollection<string>(_permissions);

		public void AddPermission(string permission)
		{
			Guard.AgainstNullOrEmptyString(permission, "permission");

			if (_permissions.Find(candidate => candidate.Equals(permission, StringComparison.InvariantCultureIgnoreCase)) != null)
			{
				return;
			}

			_permissions.Add(permission);
		}
	}
}