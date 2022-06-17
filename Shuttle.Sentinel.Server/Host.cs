using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Shuttle.Access.RestClient;
using Shuttle.Core.Container;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Core.DependencyInjection;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Mediator;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Esb.Sql.Subscription;
using Shuttle.Recall;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public void Stop()
        {
            _bus?.Dispose();
        }

        public void Configure(IHostBuilder builder)
        {
            Guard.AgainstNull(builder, nameof(builder));

            var accessClientConfiguration = AccessClientSection.GetConfiguration();

            builder.ConfigureServices(services =>
            {
                services.AddTransient<AuthenticationHeaderHandler>();
                services.AddHttpClient("AccessClient")
                    .AddHttpMessageHandler<AuthenticationHeaderHandler>()
                    .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(3));

                var registry = new ServiceCollectionComponentRegistry(services);

                registry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();

                registry.RegisterDataAccess();
                registry.RegisterInstance(SentinelServerSection.GetConfiguration());
                registry.RegisterSuffixed("Shuttle.Sentinel");
                registry.RegisterSuffixed("Shuttle.Esb.Scheduling");

                registry.RegisterInstance(AccessClientSection.GetConfiguration());
                registry.Register<IAccessClient, AccessClient>();

                registry.RegisterServiceBus();
                registry.RegisterEventStore();
                registry.RegisterEventStoreStorage();
                registry.RegisterSubscription();
                registry.RegisterMediator();
                registry.RegisterMediatorParticipants(Assembly.Load("Shuttle.Sentinel.Application"));
            });
        }

        public void Start(IServiceProvider serviceProvider)
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            var resolver = new ServiceProviderComponentResolver(serviceProvider);

            resolver.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");

            var databaseContextFactory = resolver.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            if (!databaseContextFactory.IsAvailable("Sentinel", _cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            _bus = resolver.Resolve<IServiceBus>().Start();

            using (databaseContextFactory.Create())
            {
                resolver.Resolve<IMediator>().Send(new ConfigureApplication());
            }
        }
    }
}