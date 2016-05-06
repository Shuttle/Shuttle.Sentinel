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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
	public class CorsMessageHandler : DelegatingHandler
	{
		private const string Origin = "Origin";
		private const string AccessControlRequestMethod = "Access-Control-Request-Method";
		private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
		private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
		private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
		private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

		private ILog _log;

		public CorsMessageHandler()
		{
			_log = Log.For(this);
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
															   CancellationToken cancellationToken)
		{
			return request.Headers.Contains(Origin)
					   ? ProcessCorsRequest(request, ref cancellationToken)
					   : base.SendAsync(request, cancellationToken);
		}

		private Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request,
															 ref CancellationToken cancellationToken)
		{
			if (request.Method == HttpMethod.Options)
			{
				return Task.Factory.StartNew(() =>
				{
					var response = new HttpResponseMessage(HttpStatusCode.OK);
					AddCorsResponseHeaders(request, response);
					return response;
				}, cancellationToken);
			}

			return base.SendAsync(request, cancellationToken).ContinueWith(
				task =>
				{
					var resp = task.Result;
					resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
					return resp;
				}, cancellationToken);
		}

		private void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
		{
			var origin = request.Headers.GetValues(Origin).First();

			response.Headers.Add(AccessControlAllowOrigin, origin);

			var accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
			if (accessControlRequestMethod != null)
			{
				response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
			}

			var requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
			if (!string.IsNullOrEmpty(requestedHeaders))
			{
				response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
			}
		}
	}
}