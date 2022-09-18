using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shuttle.Access;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Http;
using Shuttle.Core.DependencyInjection;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Recall;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.Queues;

namespace Shuttle.Sentinel.WebApi
{
    public class Startup
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.FromAssembly(Assembly.Load("Shuttle.Sentinel")).Add();

            services.AddHttpDatabaseContextCache();

            services.AddDataAccess(builder =>
            {
                builder.AddConnectionString("Sentinel", "System.Data.SqlClient");
                builder.Options.DatabaseContextFactory.DefaultConnectionStringName = "Sentinel";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            services.AddSingleton<IInspectionQueue, DefaultInspectionQueue>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHashingService, HashingService>();

            services.AddServiceBus(builder =>
            {
                Configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
            });

            services.AddAzureStorageQueues(builder =>
            {
                builder.AddOptions("azure", new AzureStorageQueueOptions
                {
                    ConnectionString = Configuration.GetConnectionString("azure")
                });
            });

            services.AddEventStore();
            services.AddSqlEventStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!app.ApplicationServices.GetRequiredService<IDatabaseContextFactory>()
                    .IsAvailable("Sentinel", _cancellationTokenSource.Token))
            {
                throw new ApplicationException("[connection failure]");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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