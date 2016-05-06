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
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using Newtonsoft.Json.Serialization;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Esb.Castle;
using Shuttle.Esb;
using ILog = Shuttle.Core.Infrastructure.ILog;

namespace Shuttle.Sentinel.WebApi
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : HttpApplication
	{
		private static Exception _startupException;
		private readonly ILog _log;
		private IServiceBus _bus;
		private IWindsorContainer _container;

		public WebApiApplication()
		{
			var logger = LogManager.GetLogger(typeof (WebApiApplication));

			Log.Assign(new Log4NetLog(logger));

			_log = Log.For(this);
		}

		protected void Application_Start()
		{
			try
			{
				_log.Information("[starting]");

				new ConnectionStringService().Approve();

				ConfigureWindsorContainer();

				WebApiConfiguration.Register(GlobalConfiguration.Configuration);

				GlobalConfiguration.Configuration.DependencyResolver = new ApiResolver(_container);
				GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsMessageHandler());

				_container.Register(
					Component.For<IHttpControllerActivator>().Instance(new ApiControllerActivator(_container)));

				ConfigureJson(GlobalConfiguration.Configuration);

				var serviceBusConfiguration = new ServiceBusConfiguration
				{
					MessageHandlerFactory = new CastleMessageHandlerFactory(_container)
				};

				serviceBusConfiguration.QueueManager.ScanForQueueFactories();

				_bus = new ServiceBus(serviceBusConfiguration).Start();

				_log.Information("[started]");
			}
			catch (Exception ex)
			{
				_log.Fatal("[could not start]");
				_startupException = ex;
			}
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (_startupException == null)
			{
				return;
			}

			var html = new StringBuilder();

			html.Append("<html><head><title>Shuttle Process Magaement Web-Api Startup Exception</title><style>");

			html.Append(
				"body { background: none repeat scroll 0 0 #CBE1EF; font-family: 'MS Tresbuchet',Verdana,Arial,Helvetica,sans-serif; font-size: 0.7em; margin: 0; }");
			html.Append(
				"div.header { background-color: #5c87b2; color: #ffffff; padding: .2em 2%; font-size: 2em; font-weight: bold; }");
			html.Append(
				"div.error { border: solid 1px #5a7fa5; background-color: #ffffff; color: #CC0000; padding: .5em; font-size: 2em; width: 96%; margin: .5em auto; }");
			html.Append(
				"div.information { border: solid 1px #5a7fa5; background-color: #ffffff; color: #555555; padding: 1em; font-size: 1em; width: 96%; margin: 1em auto; }");

			html.Append("</style></head><body>");
			html.Append("<div class='header'>Shuttle Process Magaement Web-Api Startup Exception</div>");
			html.AppendFormat("<div class='error'><b>source</b>:<br>{0}</div>", _startupException.Source);
			html.AppendFormat("<div class='information'><b>message</b>:<br>{0}</div>", _startupException);

			var crlf = new Regex(@"(\r\n|\r|\n)+");

			html.AppendFormat("<div class='information'><b>stack trace</b>:<br>{0}</div>",
				crlf.Replace(_startupException.StackTrace, "<br/>"));

			var reflection = _startupException as ReflectionTypeLoadException;

			if (reflection != null)
			{
				html.Append("<div class='information'><b>loader exception(s)</b>:<br>");

				foreach (var exception in reflection.LoaderExceptions)
				{
					html.AppendFormat("{0}<br/>", exception);

					var file = exception as FileNotFoundException;

					if (file == null)
					{
						return;
					}

					html.Append("[fusion log follows]<br/>");
					html.AppendFormat("{0}<br/>", file.FusionLog);
				}

				html.Append("</div>");
			}

			html.Append("</body></html>");

			HttpContext.Current.Response.Write(html);
			HttpContext.Current.Response.End();
		}

		/// <summary>
		///     Handles the End event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		private void Application_End(object sender, EventArgs e)
		{
			_log.Information("[stopping]");

			_bus?.Dispose();
			_container?.Dispose();

			_log.Information("[stopped]");

			LogManager.Shutdown();
		}


		/// <summary>
		///     Handles the Error event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		private void Application_Error(object sender, EventArgs e)
		{
			var context = HttpContext.Current;
			var encodingValue = context.Response.Headers["Content-Encoding"];

			if (encodingValue == "gzip" || encodingValue == "deflate")
			{
				context.Response.Headers.Remove("Content-Encoding");
				context.Response.Filter = null;
			}

			_log.Error(Server.GetLastError().ToString());

			var reflection = Server.GetLastError() as ReflectionTypeLoadException;

			if (reflection == null)
			{
				return;
			}

			foreach (var exception in reflection.LoaderExceptions)
			{
				_log.Error($"- '{exception.Message}'.");

				var file = exception as FileNotFoundException;

				if (file == null)
				{
					return;
				}

				_log.Error("[fusion log follows]:");
				_log.Error(file.FusionLog);
			}
		}

		private static void ConfigureJson(HttpConfiguration configuration)
		{
			configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
				new CamelCasePropertyNamesContractResolver();
		}

		private void ConfigureWindsorContainer()
		{
			_container = new WindsorContainer();

			_container.RegisterDataAccessCore();
			_container.RegisterDataAccess("Shuttle.Sentinel");

			_container.Register(Component.For<IDatabaseContextCache>().ImplementedBy<ThreadStaticDatabaseContextCache>());

			_container.RegisterConfiguration(SentinelSection.Configuration());

			_container.Register("Shuttle.Sentinel.WebApi", typeof (ApiController), "Controller");
			_container.Register("Shuttle.Sentinel", "Service");
		}
	}
}