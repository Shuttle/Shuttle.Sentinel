using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.Module
{
    public class Bootstrap :
        IComponentRegistryBootstrap,
        IComponentResolverBootstrap
    {
        private static bool _registryBootstrapCalled;
        private static bool _resolverBootstrapCalled;

        public void Register(IComponentRegistry registry)
        {
            Guard.AgainstNull(registry, "registry");

            if (_registryBootstrapCalled)
            {
                return;
            }

            _registryBootstrapCalled = true;

            if (!registry.IsRegistered<ISentinelConfiguration>())
            {
                registry.AttemptRegister(SentinelSection.Configuration());
            }

            registry.AttemptRegister<IMetricCollector, MetricCollector>();
            registry.AttemptRegister<SentinelModule>();
        }

        public void Resolve(IComponentResolver resolver)
        {
            Guard.AgainstNull(resolver, "resolver");

            if (_resolverBootstrapCalled)
            {
                return;
            }

            resolver.Resolve<SentinelModule>();

            _resolverBootstrapCalled = true;
        }
    }
}