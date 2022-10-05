using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using System;

namespace Shuttle.Sentinel.Module
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSentinelModule(this IServiceCollection services,
            Action<SentinelBuilder> builder = null)
        {
            Guard.AgainstNull(services, nameof(services));

            var sentinelModuleBuilder = new SentinelBuilder(services);

            builder?.Invoke(sentinelModuleBuilder);

            services.TryAddSingleton<ISentinelObserver, SentinelObserver>();
            services.TryAddSingleton<IEndpointAggregator, EndpointAggregator>();

            services.AddOptions<SentinelOptions>().Configure(options =>
            {
                options.Enabled = sentinelModuleBuilder.Options.Enabled;
                options.HeartbeatIntervalDuration = sentinelModuleBuilder.Options.HeartbeatIntervalDuration;
                options.MaximumMessageContentSize = sentinelModuleBuilder.Options.MaximumMessageContentSize;
                options.TransientInstance = sentinelModuleBuilder.Options.TransientInstance;
                options.Tags = sentinelModuleBuilder.Options.Tags;
            });

            services.AddPipelineModule<SentinelModule>();

            return services;
        }
    }
}