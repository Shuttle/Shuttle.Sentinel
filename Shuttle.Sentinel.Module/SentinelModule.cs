using System;
using System.Threading;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IThreadState
    {
        private readonly ISentinelConfiguration _configuration;
        private readonly string _inboxMessagePipelineName = typeof(InboxMessagePipeline).FullName;
        private readonly IMetricCollector _metricCollector;
        private readonly SentinelObserver _observer;
        private readonly Thread _thread;
        private volatile bool _active = true;

        private DateTime _nextHeartbeat;

        public SentinelModule(IPipelineFactory pipelineFactory, IServiceBusEvents serviceBusEvents,
            ISentinelConfiguration configuration, IMetricCollector metricCollector, SentinelObserver observer)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(serviceBusEvents, nameof(serviceBusEvents));
            Guard.AgainstNull(configuration, nameof(configuration));
            Guard.AgainstNull(metricCollector, nameof(metricCollector));
            Guard.AgainstNull(observer, nameof(observer));

            pipelineFactory.PipelineCreated += PipelineCreated;
            serviceBusEvents.Stopping += (sender, args) => { _active = false; };

            _configuration = configuration;
            _metricCollector = metricCollector;
            _observer = observer;

            SetNextHeartbeat();

            _thread = new Thread(HeartbeatProcessing);
            _thread.Start();
        }

        public bool Active => _active;

        private void SetNextHeartbeat()
        {
            _nextHeartbeat = DateTime.Now.AddSeconds(_configuration.HeartbeatIntervalSeconds);
        }

        private void HeartbeatProcessing()
        {
            while (_active)
            {
                if (DateTime.Now >= _nextHeartbeat)
                {
                    _metricCollector.SendMetrics();

                    SetNextHeartbeat();
                }

                ThreadSleep.While(250, this);
            }

            _thread.Join(TimeSpan.FromSeconds(5));
        }

        private void PipelineCreated(object sender, PipelineEventArgs e)
        {
            var pipelineName = e.Pipeline.GetType().FullName;

            if (!pipelineName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            e.Pipeline.RegisterObserver(_observer);
        }
    }
}