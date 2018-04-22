using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private WindsorContainer _container;

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _container = new WindsorContainer();

            var container = new WindsorComponentContainer(_container);

            container.RegisterSuffixed("Shuttle.Sentinel");

            EventStore.Register(container);
            ServiceBus.Register(container);

            container.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");
            container.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = ServiceBus.Create(container).Start();
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}