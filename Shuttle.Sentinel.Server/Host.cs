using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Data;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Esb;
using Shuttle.Esb.Castle;
using Shuttle.Esb.SqlServer;
using Shuttle.Recall;
using Shuttle.Recall.SqlServer;
using Shuttle.Sentinel.Messages.v1;

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
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof (Host))));

            _container = new WindsorContainer();

            _container.RegisterDataAccessCore();
            _container.RegisterDataAccess("Shuttle.Sentinel");

            _container.Register(Component.For<ISerializer>().ImplementedBy<DefaultSerializer>());
            _container.Register(Component.For<ISubscriptionManager>().Instance(SubscriptionManager.Default()));
            _container.Register(Component.For<IDatabaseContextCache>().ImplementedBy<ThreadStaticDatabaseContextCache>());
            _container.Register(Component.For<IHashingService>().ImplementedBy<HashingService>());
            _container.RegisterConfiguration(SentinelSection.Configuration());

            var subscriptionManager = _container.Resolve<ISubscriptionManager>();

            _bus = ServiceBus.Create(
                c =>
                {
                    c.MessageHandlerFactory(new CastleMessageHandlerFactory(_container).RegisterHandlers());
                    c.SubscriptionManager(subscriptionManager);
                }).Start();
        }
    }
}