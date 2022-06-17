using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shuttle.Access;
using Shuttle.Access.DataAccess;
using Shuttle.Access.Mvc.Rest;
using Shuttle.Access.RestClient;
using Shuttle.Access.Sql;
using Shuttle.Core.Configuration;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Http;
using Shuttle.Core.DependencyInjection;
using Shuttle.Core.Logging;
using Shuttle.Core.Reflection;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.OAuth;
using Shuttle.Recall;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Queues;
using Shuttle.Sentinel.WebApi.Configuration;

namespace Shuttle.Sentinel.WebApi
{
    public class Startup
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ILog _log;
        private IServiceBus _bus;

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
            services.AddSingleton(AccessClientSection.GetConfiguration());
            services.AddSingleton<IAccessClient, AccessClient>();

            services.AddSingleton(AccessSessionSection.GetConfiguration());
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
            services.AddSingleton<IAccessService, RestAccessService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Shuttle.Sentinel.WebApi", Version = "v1" });

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "access-session-token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Shuttle.Access token security",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };

                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };

                options.AddSecurityRequirement(requirement);
            });

            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            var registry = new ServiceCollectionComponentRegistry(services);

            registry.RegisterSuffixed("Shuttle.Sentinel");
            registry.RegisterSuffixed("Shuttle.Access.Sql");
            registry.RegisterSuffixed("Shuttle.Esb.Scheduling");

            registry.Register<IInspectionQueue, DefaultInspectionQueue>();
            registry.Register<IHttpContextAccessor, HttpContextAccessor>();
            registry.Register<IDatabaseContextCache, ContextDatabaseContextCache>();
            registry.Register<IHashingService, HashingService>();
            registry.RegisterInstance(AccessClientSection.GetConfiguration());
            registry.Register<IAccessClient, AccessClient>();
            registry.Register<IWebApiConfiguration, WebApiConfiguration>();

            registry.RegisterInstance(
                OAuthConfigurationProvider.Open(
                    ConfigurationItem<string>.ReadSetting("oauth-credentials-path").GetValue()));
            registry.RegisterDataAccess();
            registry.RegisterServiceBus();
            registry.RegisterEventStore();
            registry.RegisterEventStoreStorage();
            //componentContainer.RegisterMediator();
            //componentContainer.RegisterMediatorParticipants(Assembly.Load("Shuttle.Access.Application"));

            registry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {

            var applicationPartManager = app.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            //var controllerFeature = new ControllerFeature();

            //applicationPartManager.PopulateFeature(controllerFeature);

            //foreach (var type in controllerFeature.Controllers.Select(t => t.AsType()))
            //{
            //    componentContainer.Register(type, type);
            //}

            app.ApplicationServices.GetRequiredService<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");

            var databaseContextFactory =
                app.ApplicationServices.GetRequiredService<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            if (!databaseContextFactory.IsAvailable("Sentinel", _cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            _bus = app.ApplicationServices.GetRequiredService<IServiceBus>().Start();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(error =>
            {
                error.Run(context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature != null)
                    {
                        _log.Error(feature.Error.AllMessages());
                    }

                    return Task.CompletedTask;
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shuttle.Access.WebApi.v1");
            });
        }
    }
}