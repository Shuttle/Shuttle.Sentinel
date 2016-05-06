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

using System.Web.Http;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
	public class SessionsController : ApiController
	{
		private readonly ISessionService _sessionService;

		public SessionsController(ISessionService sessionService)
		{
			Guard.AgainstNull(sessionService, "sessionService");

			_sessionService = sessionService;
		}

		public IHttpActionResult Post([FromBody] RegisterSessionModel model)
		{
			Guard.AgainstNull(model, "model");

			var registerSessionResult = _sessionService.Register(model.EMail, model.Password);

			return registerSessionResult.Ok
				? (IHttpActionResult) Ok(new
				{
					Registered = true,
					Token = registerSessionResult.Token.ToString("n"),
					registerSessionResult.Permissions
				})
				: Ok(new
				{
					Registered = false
				});
		}
	}
}