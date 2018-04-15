using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IDisposable, IThreadState
    {
        private readonly ISentinelConfiguration _sentinelConfiguration;
        private readonly string _shutdownPipelineName = typeof(ShutdownPipeline).FullName;
        private readonly string _startupPipelineName = typeof(StartupPipeline).FullName;
        private volatile bool _active;

        public SentinelModule(IPipelineFactory pipelineFactory, ISentinelConfiguration sentinelConfiguration)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(sentinelConfiguration, nameof(sentinelConfiguration));

            _sentinelConfiguration = sentinelConfiguration;

            pipelineFactory.PipelineCreated += PipelineCreated;
        }

        public void Dispose()
        {
            _active = false;
        }

        public bool Active => _active;

        private void PipelineCreated(object sender, PipelineEventArgs e)
        {
            var pipelineName = e.Pipeline.GetType().FullName ?? string.Empty;

            if (pipelineName.Equals(_startupPipelineName, StringComparison.InvariantCultureIgnoreCase)
                ||
                pipelineName.Equals(_shutdownPipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            //e.Pipeline.RegisterObserver(new SentinelObserver(this, _sentinel));
        }
    }
}