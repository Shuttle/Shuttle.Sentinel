using System;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Data;
using Shuttle.Core.DependencyInjection;
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    services.FromAssembly(Assembly.Load("Shuttle.Sentinel")).Add();

                    services.AddDataAccess(builder =>
                    {
                        builder.AddConnectionString("Sentinel", "Microsoft.Data.SqlClient");
                        builder.Options.DatabaseContextFactory.DefaultConnectionStringName = "Sentinel";
                    });

                    services.AddSqlEventStorage();
                    services.AddSqlEventProcessing(builder =>
                    {
                        builder.Options.EventProjectionConnectionStringName = "Sentinel";
                        builder.Options.EventStoreConnectionStringName = "Sentinel";
                    });

                    services.AddEventStore(builder =>
                    {
                        builder.AddEventHandler<ProfileHandler>("Profile");
                    });
                })
                .Build();

            var databaseContextFactory = host.Services.GetRequiredService<IDatabaseContextFactory>();

            var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += delegate
            {
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

            host.Run();
        }
    }
}