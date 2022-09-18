using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;
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

            services.AddOptions<SentinelOptions>().Configure(options =>
            {
                options.Enabled = sentinelModuleBuilder.Options.Enabled;
                options.HeartbeatIntervalDuration = sentinelModuleBuilder.Options.HeartbeatIntervalDuration;
            });

            return services;
        }
    }
}