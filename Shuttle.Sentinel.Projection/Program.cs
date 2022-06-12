using System;
using System.Data.Common;
using System.Text;
using System.Threading;
using log4net;
using Microsoft.Data.SqlClient;
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Ninject;
using Shuttle.Core.Reflection;
using Shuttle.Core.ServiceHost;
using Shuttle.Recall;
using Shuttle.Recall.Sql.EventProcessing;
using Shuttle.Recall.Sql.Storage;

namespace Shuttle.Sentinel.Projection
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ServiceHost.Run<Host>();
        }
    }

    public class Host : IServiceHost
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private IEventProcessor _eventProcessor;
        private IEventStore _eventStore;
        private IKernel _kernel;

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _kernel = new StandardKernel();

            var container = new NinjectComponentContainer(_kernel);

            container.RegisterDataAccess();
            container.RegisterSuffixed("Shuttle.Sentinel");
            container.RegisterEventStore();
            container.RegisterEventStoreStorage();
            container.RegisterEventProcessing();

            _ = container.Resolve<EventProcessingModule>();

            _eventStore = container.Resolve<IEventStore>();
            _eventProcessor = container.Resolve<IEventProcessor>();

            var databaseContextFactory = container.Resolve<IDatabaseContextFactory>();

            if (!databaseContextFactory.IsAvailable("Sentinel", _cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            using (container.Resolve<IDatabaseContextFactory>().Create("Sentinel"))
            {
                _eventProcessor.AddProjection("Profile");

                container.AddEventHandler<ProfileHandler>("Profile");
            }

            _eventProcessor.Start();
        }

        public void Stop()
        {
            _kernel?.Dispose();
            _eventProcessor?.Dispose();
            _eventStore?.AttemptDispose();
        }
    }
}