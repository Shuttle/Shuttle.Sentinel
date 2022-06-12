using log4net;
using Ninject;
using Shuttle.Access.Api;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Ninject;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Esb.Sql.Subscription;
using Shuttle.Recall;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private IKernel _kernel;

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _kernel = new StandardKernel();

            var container = new NinjectComponentContainer(_kernel);

            container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();

            container.RegisterDataAccess();
            container.RegisterInstance(SentinelServerSection.GetConfiguration());
            container.RegisterSuffixed("Shuttle.Sentinel");
            container.RegisterSuffixed("Shuttle.Esb.Scheduling");

            container.RegisterInstance(AccessClientSection.GetConfiguration());
            container.Register<IAccessClient, AccessClient>();

            container.RegisterServiceBus();
            container.RegisterEventStore();
            container.RegisterEventStoreStorage();
            container.RegisterSubscription();

            container.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");
            container.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = container.Resolve<IServiceBus>().Start();
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}