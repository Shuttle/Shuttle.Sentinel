using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Shuttle.Access.RestClient;
using Shuttle.Core.Data;
using Shuttle.Core.DependencyInjection;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Esb.Sql.Subscription;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Recall;
using Shuttle.Core.Mediator;
using System.Threading;
using System;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class Program
    {
        private static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    services.AddTransient<AuthenticationHeaderHandler>();

                    services.FromAssembly(Assembly.Load("Shuttle.Sentinel")).Add();

                    services.Configure<ServerOptions>(configuration.GetSection(ServerOptions.SectionName));

                    services.AddDataAccess(builder =>
                    {
                        builder.AddConnectionString("Sentinel", "Microsoft.Data.SqlClient");
                        builder.Options.DatabaseContextFactory.DefaultConnectionStringName = "Sentinel";
                    });

                    services.AddHttpClient("AccessClient")
                        .AddHttpMessageHandler<AuthenticationHeaderHandler>()
                        .AddTransientHttpErrorPolicy(policyBuilder =>
                            policyBuilder.RetryAsync(3));

                    services.AddAccessClient(builder =>
                    {
                        configuration.GetSection(AccessClientOptions.SectionName).Bind(builder.Options);
                    });

                    services.AddServiceBus(builder =>
                    {
                        configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
                    });

                    services.AddAzureStorageQueues(builder =>
                    {
                        builder.AddOptions("azure", new AzureStorageQueueOptions
                        {
                            ConnectionString = configuration.GetConnectionString("azure")
                        });
                    });

                    services.AddEventStore();
                    services.AddSqlEventStorage();
                    services.AddSqlSubscription();

                    services.AddMediator(builder =>
                    {
                        builder.AddParticipants(Assembly.Load("Shuttle.Sentinel.Application"));
                    });
                })
                .Build();

            var databaseContextFactory = host.Services.GetRequiredService<IDatabaseContextFactory>();

            var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += delegate {
                cancellationTokenSource.Cancel();
            };

            if (!databaseContextFactory.IsAvailable("Sentinel", cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            using (databaseContextFactory.Create())
            {
                host.Services.GetRequiredService<IMediator>().Send(new ConfigureApplication(), cancellationTokenSource.Token);
            }

            host.Run();
        }
    }

    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}