using System;
using System.Data.Common;
using System.Text;
using System.Threading;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.DependencyInjection;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Reflection;
using Shuttle.Core.WorkerService;
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

        public void Stop()
        {
            _eventProcessor?.Dispose();
            _eventStore?.AttemptDispose();
        }

        public void Start(IServiceProvider serviceProvider)
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            var resolver = new ServiceProviderComponentResolver(serviceProvider);

            _ = serviceProvider.GetRequiredService<EventProcessingModule>();

            _eventStore = serviceProvider.GetRequiredService<IEventStore>();
            _eventProcessor = serviceProvider.GetRequiredService<IEventProcessor>();

            var databaseContextFactory = serviceProvider.GetRequiredService<IDatabaseContextFactory>();

            if (!databaseContextFactory.IsAvailable("Sentinel", _cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            using (serviceProvider.GetRequiredService<IDatabaseContextFactory>().Create("Sentinel"))
            {
                _eventProcessor.AddProjection("Profile");

                resolver.AddEventHandler<ProfileHandler>("Profile");
            }

            _eventProcessor.Start();
        }

        public void Configure(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var container = new ServiceCollectionComponentRegistry(services);

                container.RegisterDataAccess();
                container.RegisterSuffixed("Shuttle.Sentinel");
                container.RegisterEventStore();
                container.RegisterEventStoreStorage();
                container.RegisterEventProcessing();
            });
        }
    }
}