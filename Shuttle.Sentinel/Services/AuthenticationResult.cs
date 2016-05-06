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
	public enum AuthenticationResultType
	{
		Authenticated = 0,
		ActivationRequired = 1,
		PasswordExpired = 2,
		Failure = 3
	}

	public class AuthenticationResult
	{
		private AuthenticationResult(bool authenticated, AuthenticationResultType resultType, object authenticationTag)
		{
			Authenticated = authenticated;
			ResultType = resultType;
			AuthenticationTag = authenticationTag;
		}

		public bool Authenticated { get; private set; }
		public AuthenticationResultType ResultType { get; private set; }
		public object AuthenticationTag { get; private set; }

		public static AuthenticationResult Success()
		{
			return new AuthenticationResult(true, AuthenticationResultType.Authenticated, null);
		}

		public static AuthenticationResult Success(object authenticationTag)
		{
			return new AuthenticationResult(true, AuthenticationResultType.Authenticated, authenticationTag);
		}

		public static AuthenticationResult Failure()
		{
			return new AuthenticationResult(false, AuthenticationResultType.Failure, null);
		}

		public static AuthenticationResult Failure(AuthenticationResultType resultType)
		{
			Guard.Against<ApplicationException>(resultType == AuthenticationResultType.Authenticated, "Cannot specify 'Authenticated' as the AuthenticationResultType when authentication has failed.");

			return new AuthenticationResult(false, resultType, null);
		}
	}
}