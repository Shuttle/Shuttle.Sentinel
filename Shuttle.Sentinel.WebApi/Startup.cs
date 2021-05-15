using System.Linq;
using Castle.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Access;
using Shuttle.Access.Api;
using Shuttle.Access.DataAccess;
using Shuttle.Access.Mvc.DataStore;
using Shuttle.Access.Sql;
using Shuttle.Core.Castle;
using Shuttle.Core.Configuration;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Http;
using Shuttle.Core.Logging;
using Shuttle.Core.Reflection;
using Shuttle.Esb;
using Shuttle.OAuth;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Queues;
using Shuttle.Sentinel.WebApi.Configuration;

namespace Shuttle.Sentinel.WebApi
{
    public class Startup
    {
        private IServiceBus _bus;
        private readonly ILog _log;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _log = Log.For(this);
        }

        public IConfiguration Configuration { get; }

        private void OnShutdown()
        {
            _bus?.Dispose();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IWindsorContainer>(new WindsorContainer());
            services.AddSingleton<IControllerActivator, ControllerActivator>();

            services.AddSingleton(AccessSection.Configuration());
            services.AddSingleton(SentinelSection.Configuration());
            services.AddSingleton<IConnectionConfigurationProvider, ConnectionConfigurationProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDatabaseContextCache, ContextDatabaseContextCache>();
            services.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
            services.AddSingleton<IDatabaseGateway, DatabaseGateway>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbCommandFactory, DbCommandFactory>();
            services.AddSingleton<IDataRowMapper, DataRowMapper>();
            services.AddSingleton<IQueryMapper, QueryMapper>();
            services.AddSingleton<ISessionQueryFactory, SessionQueryFactory>();
            services.AddSingleton<ISessionQuery, SessionQuery>();
            services.AddSingleton<ISessionRepository, SessionRepository>();
            services.AddSingleton<IDataRowMapper<Session>, SessionMapper>();
            services.AddSingleton(typeof(IDataRepository<>), typeof(DataRepository<>));
            services.AddSingleton<IAccessService, DataStoreAccessService>();

            services.AddMvc();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            var container = app.ApplicationServices.GetService<IWindsorContainer>();

            var componentContainer = new WindsorComponentContainer(container);

            componentContainer.RegisterSuffixed("Shuttle.Sentinel");
            componentContainer.RegisterSuffixed("Shuttle.Access.Sql");
            componentContainer.RegisterSuffixed("Shuttle.Esb.Scheduling");

            componentContainer.Register<IInspectionQueue, DefaultInspectionQueue>();
            componentContainer.Register<IHttpContextAccessor, HttpContextAccessor>();
            componentContainer.Register<IDatabaseContextCache, ContextDatabaseContextCache>();
            componentContainer.Register<IHashingService, HashingService>();
            componentContainer.RegisterInstance(AccessClientSection.GetConfiguration());
            componentContainer.Register<IAccessClient, AccessClient>();

            componentContainer.RegisterInstance(app.ApplicationServices.GetService<IAccessConfiguration>());
            componentContainer.RegisterInstance(app.ApplicationServices.GetService<ISentinelConfiguration>());
            componentContainer.Register<IWebApiConfiguration, WebApiConfiguration>();

            componentContainer.RegisterInstance(
                OAuthConfigurationProvider.Open(
                    ConfigurationItem<string>.ReadSetting("oauth-credentials-path").GetValue()));

            var applicationPartManager = app.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            var controllerFeature = new ControllerFeature();

            applicationPartManager.PopulateFeature(controllerFeature);

            foreach (var type in controllerFeature.Controllers.Select(t => t.AsType()))
            {
                componentContainer.Register(type, type);
            }

            ServiceBus.Register(componentContainer);
            EventStore.Register(componentContainer);

            componentContainer.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");
            componentContainer.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = ServiceBus.Create(componentContainer).Start();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature != null)
                    {
                        _log.Error(feature.Error.AllMessages());
                    }
                });
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}