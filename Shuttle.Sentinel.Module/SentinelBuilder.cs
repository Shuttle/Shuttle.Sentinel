using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;
using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelBuilder
    {
        private SentinelOptions _sentinelOptions = new SentinelOptions();

        public SentinelBuilder(IServiceCollection services)
        {
            Guard.AgainstNull(services, nameof(services));

            Services = services;
        }

        public SentinelOptions Options
        {
            get => _sentinelOptions;
            set => _sentinelOptions = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IServiceCollection Services { get; }
    }
}