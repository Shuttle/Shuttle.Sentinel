using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Data;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Recall;
using Shuttle.Recall.SqlServer;

namespace Shuttle.Sentinel.EventProcessing.Server
{
	public class Host : IHost, IDisposable
	{
		private WindsorContainer _container;
		private IEventProcessor _processor;

		public void Dispose()
		{
			_processor.Dispose();
		}

		public void Start()
		{
			Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof (Host))));

			_container = new WindsorContainer();

			_container.RegisterDataAccessCore();
			_container.RegisterDataAccess("Shuttle.Sentinel");
			_container.RegisterDataAccess("Shuttle.Recall.SqlServer");

			_container.Register(Component.For<IDatabaseContextCache>().ImplementedBy<ThreadStaticDatabaseContextCache>());
			_container.Register(Component.For<IEventStore>().ImplementedBy<EventStore>());
			_container.Register(Component.For<IKeyStore>().ImplementedBy<KeyStore>());
			_container.Register(Component.For<ISerializer>().ImplementedBy<DefaultSerializer>());
			_container.Register(Component.For<IEventStoreQueryFactory>().ImplementedBy<EventStoreQueryFactory>());
			_container.Register(Component.For<IKeyStoreQueryFactory>().ImplementedBy<KeyStoreQueryFactory>());
			_container.Register(Component.For<IProjectionService>().ImplementedBy<ProjectionService>());
			_container.Register(Component.For<IProjectionConfiguration>().Instance(ProjectionSection.Configuration()));
			_container.RegisterConfiguration(SentinelSection.Configuration());

			_container.Register(Component.For<object>().ImplementedBy<UserProjectionHandler>().Named("UserProjectionHandler"));

			_processor = EventProcessor.Create(c => { c.ProjectionService(_container.Resolve<IProjectionService>()); });

			_processor.AddEventProjection(new EventProjection("User").AddEventHandler(_container.Resolve<object>("UserProjectionHandler")));

			_processor.Start();
		}
	}
}