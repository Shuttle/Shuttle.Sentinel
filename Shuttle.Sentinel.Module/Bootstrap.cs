using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.Module
{
    public class Bootstrap :
        IComponentRegistryBootstrap,
        IComponentResolverBootstrap
    {
        private static bool _registryBootstrapCalled;
        private static bool _resolverBootstrapCalled;
        private static bool _registered;

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
                var sentinelConfiguration = SentinelSection.Configuration();

                if (sentinelConfiguration == null)
                {
                    Log.Information("No configuration has been registered for the Sentinel module and/or the configuration has not been specified.  The module will not be activated.");

                    return;
                }

                registry.AttemptRegister(sentinelConfiguration);
            }

            registry.AttemptRegister<IMetricCollector, MetricCollector>();
            registry.AttemptRegister<DispatchPipelineObserver>();
            registry.AttemptRegister<InboxPipelineObserver>();
            registry.AttemptRegister<SentinelModule>();

            _registered = true;
        }

        public void Resolve(IComponentResolver resolver)
        {
            Guard.AgainstNull(resolver, "resolver");

            if (_resolverBootstrapCalled || !_registered)
            {
                return;
            }

            resolver.Resolve<SentinelModule>();

            _resolverBootstrapCalled = true;
        }
    }
}