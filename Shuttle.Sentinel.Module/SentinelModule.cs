using System;
using System.Reflection;
using System.Threading;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IThreadState
    {
        private readonly string _inboxMessagePipelineName = typeof(InboxMessagePipeline).FullName;
        private readonly string _dispatchTransportMessagePipelineName = typeof(DispatchTransportMessagePipeline).FullName;

        private readonly ISentinelConfiguration _sentinelConfiguration;
        private readonly IMetricCollector _metricCollector;
        private readonly InboxPipelineObserver _inboxPipelineObserver;
        private readonly DispatchPipelineObserver _dispatchPipelineObserver;
        private readonly Thread _thread;
        private volatile bool _active = true;

        private DateTime _nextHeartbeat;

        public SentinelModule(IServiceBus serviceBus, IServiceBusConfiguration serviceBusConfiguration, IServiceBusEvents serviceBusEvents, ISentinelConfiguration sentinelConfiguration, IPipelineFactory pipelineFactory, IMetricCollector metricCollector, InboxPipelineObserver inboxPipelineObserver, DispatchPipelineObserver dispatchPipelineObserver)
        {
            Guard.AgainstNull(serviceBus, nameof(serviceBus));
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(serviceBusEvents, nameof(serviceBusEvents));
            Guard.AgainstNull(sentinelConfiguration, nameof(sentinelConfiguration));
            Guard.AgainstNull(metricCollector, nameof(metricCollector));
            Guard.AgainstNull(inboxPipelineObserver, nameof(inboxPipelineObserver));
            Guard.AgainstNull(dispatchPipelineObserver, nameof(dispatchPipelineObserver));

            var entryAssembly = Assembly.GetEntryAssembly();

            pipelineFactory.PipelineCreated += PipelineCreated;

            serviceBusEvents.Stopping += (sender, args) => { _active = false; };
            serviceBusEvents.Started += (sender, args) =>
            {
                serviceBus.Send(new RegisterEndpointCommand
                {
                    EndpointName = _sentinelConfiguration.EndpointName,
                    MachineName = _sentinelConfiguration.MachineName,
                    BaseDirectory = _sentinelConfiguration.BaseDirectory,
                    EntryAssemblyQualifiedName = entryAssembly != null ? entryAssembly.ToString() : "(null)",
                    IPv4Address = _sentinelConfiguration.IPv4Address,
                    InboxWorkQueueUri = serviceBusConfiguration.HasInbox
                        ? serviceBusConfiguration.Inbox.WorkQueueUri
                        : string.Empty,
                    ControlInboxWorkQueueUri = serviceBusConfiguration.HasControlInbox
                        ? serviceBusConfiguration.ControlInbox.WorkQueueUri
                        : string.Empty
                }, c => c.WithRecipient(sentinelConfiguration.InboxWorkQueueUri));
            };

            _sentinelConfiguration = sentinelConfiguration;
            _metricCollector = metricCollector;
            _inboxPipelineObserver = inboxPipelineObserver;
            _dispatchPipelineObserver = dispatchPipelineObserver;

            _thread = new Thread(HeartbeatProcessing);
            _thread.Start();
        }

        public bool Active => _active;

        private void SetNextHeartbeat()
        {
            _nextHeartbeat = DateTime.Now.AddSeconds(_sentinelConfiguration.HeartbeatIntervalSeconds);
        }

        private void HeartbeatProcessing()
        {
            SetNextHeartbeat();

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

            if (pipelineName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                e.Pipeline.RegisterObserver(_inboxPipelineObserver);
            }

            if (pipelineName.Equals(_dispatchTransportMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                e.Pipeline.RegisterObserver(_dispatchPipelineObserver);
            }
        }
    }
}