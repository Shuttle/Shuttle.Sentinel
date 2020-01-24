using System.Linq;
using Castle.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Access;
using Shuttle.Access.DataAccess;
using Shuttle.Access.Sql;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Http;
using Shuttle.Core.Data.SqlClient;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Queues;
using Shuttle.Sentinel.WebApi.Configuration;

namespace Shuttle.Sentinel.WebApi
{
    public class Startup
    {
        private IServiceBus _bus;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddSingleton<IDbProviderFactories, DbProviderFactories>();
            services.AddSingleton<IConnectionConfigurationProvider, ConnectionConfigurationProvider>();
            services.AddSingleton<IDatabaseContextCache, ContextDatabaseContextCache>();
            services.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
            services.AddSingleton<IDatabaseGateway, DatabaseGateway>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbCommandFactory, DbCommandFactory>();
            services.AddSingleton<IQueryMapper, QueryMapper>();
            services.AddSingleton<ISessionQueryFactory, SessionQueryFactory>();
            services.AddSingleton<ISessionQuery, SessionQuery>();

            services.AddMvc();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime)
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

            componentContainer.RegisterInstance(app.ApplicationServices.GetService<IAccessConfiguration>());
            componentContainer.RegisterInstance(app.ApplicationServices.GetService<ISentinelConfiguration>());

            var applicationPartManager = app.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            var controllerFeature = new ControllerFeature();

            applicationPartManager.PopulateFeature(controllerFeature);

            foreach (var type in controllerFeature.Controllers.Select(t => t.AsType()))
            {
                componentContainer.Register(type, type);
            }

            ServiceBus.Register(componentContainer);

            componentContainer.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");
            componentContainer.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = ServiceBus.Create(componentContainer).Start();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseMvc();
        }
    }
}