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
using System.Reflection;
using System.Web.Http;

namespace Shuttle.Sentinel.WebApi
{
	public class ServerController : ApiController
	{
		[HttpGet]
		[Route("api/server/configuration")]
		public IHttpActionResult GetServerConfiguration()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;

			return Ok(new
			{
				Version = $"{version.Major}.{version.Minor}.{version.Build}"
			});
		}
	}
}