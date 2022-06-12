using Shuttle.Core.Container;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Module
{
    public static class ComponentRegistryExtensions
    {
        public static void RegisterSentinel(IComponentRegistry registry)
        {
            Guard.AgainstNull(registry, nameof(registry));

            if (!registry.IsRegistered<ISentinelConfiguration>())
            {
                registry.AttemptRegisterInstance(SentinelSection.Configuration());
            }

            registry.AttemptRegister<ISentinelObserver, SentinelObserver>();
            registry.AttemptRegister<IEndpointAggregator, EndpointAggregator>();
            registry.AttemptRegister<SentinelModule>();
        }
    }
}