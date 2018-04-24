using Shuttle.Core.Container;
using Shuttle.Core.Contract;

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
            Guard.AgainstNull(registry, nameof(registry));

            if (_registryBootstrapCalled)
            {
                return;
            }

            _registryBootstrapCalled = true;

            if (!registry.IsRegistered<ISentinelConfiguration>())
            {
                registry.AttemptRegisterInstance(SentinelSection.Configuration());
            }

            registry.AttemptRegister<ISentinelObserver, SentinelObserver>();
            registry.AttemptRegister<IEndpointAggregator, EndpointAggregator>();
            registry.AttemptRegister<SentinelModule>();
        }

        public void Resolve(IComponentResolver resolver)
        {
            Guard.AgainstNull(resolver, nameof(resolver));

            if (_resolverBootstrapCalled)
            {
                return;
            }

            resolver.Resolve<SentinelModule>();

            _resolverBootstrapCalled = true;
        }
    }
}