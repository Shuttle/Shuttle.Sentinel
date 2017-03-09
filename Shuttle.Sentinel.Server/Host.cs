using System;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Data;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Esb;
using Shuttle.Esb.Msmq;
using Shuttle.Esb.RabbitMQ;
using Shuttle.Esb.Sql;

namespace Shuttle.Sentinel.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;
		private WindsorContainer _container;

		public void Dispose()
		{
			_bus.Dispose();
		}

		public void Start()
		{
			Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

			_container = new WindsorContainer();

			_container.RegisterDataAccessCore();
			_container.RegisterDataAccess("Shuttle.Sentinel");

			var container = new WindsorComponentContainer(_container);

			// TODO: load these dynamically somehow
			container.Register<IRabbitMQConfiguration, RabbitMQConfiguration>();
			container.Register<IMsmqConfiguration, MsmqConfiguration>();

			container.Register<IDatabaseContextCache, ThreadStaticDatabaseContextCache>();

			container.Register<Esb.Sql.IScriptProviderConfiguration, Esb.Sql.ScriptProviderConfiguration>();
			container.Register<Esb.Sql.IScriptProvider, Esb.Sql.ScriptProvider>();

			container.Register<Recall.Sql.IScriptProviderConfiguration, Recall.Sql.ScriptProviderConfiguration>();
			container.Register<Recall.Sql.IScriptProvider, Recall.Sql.ScriptProvider>();

			container.Register<ISqlConfiguration>(SqlSection.Configuration());
			container.Register<ISubscriptionManager, SubscriptionManager>();

			ServiceBus.Register(container);

			_bus = ServiceBus.Create(container).Start();
		}
	}
}