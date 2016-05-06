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

namespace Shuttle.Sentinel
{
	public class RegisterSessionResult
	{
		private RegisterSessionResult(Guid token, IEnumerable<string> permissions)
		{
			Permissions = permissions;
			Token = token;
		}

		public Guid Token { get; private set; }
		public IEnumerable<string> Permissions { get; private set; }
		public bool Ok => !Guid.Empty.Equals(Token);

		public static RegisterSessionResult Success(Guid token, IEnumerable<string> permissions)
		{
			return new RegisterSessionResult(token, permissions);
		}

		public static RegisterSessionResult Failure()
		{
			return new RegisterSessionResult(Guid.Empty, new List<string>());
		}
	}
}