using System;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Data;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Esb;
using Shuttle.Recall;

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

            EventStore.Register(container);
            ServiceBus.Register(container);

            container.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = ServiceBus.Create(container).Start();
        }
    }
}