using System;
using System.Linq;
using System.Threading;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IDisposable, IPipelineObserver<OnStarted>
    {
        private readonly string _inboxMessagePipelineName = typeof(InboxMessagePipeline).FullName;
        private readonly string _dispatchTransportMessagePipelineName = typeof(DispatchTransportMessagePipeline).FullName;
        private readonly string _startupPipelineName = typeof(StartupPipeline).FullName;

        private readonly IServiceBus _bus;
        private readonly IMessageRouteProvider _messageRouteProvider;
        private readonly IEndpointAggregator _endpointAggregator;
        private readonly ISentinelObserver _sentinelObserver;
        private readonly ISentinelConfiguration _sentinelConfiguration;
        private volatile bool _active;
        private Thread _thread;
        private DateTime _nextSendDate = DateTime.Now;
        private CancellationToken _cancellationToken;

        public SentinelModule(IServiceBus bus, IMessageRouteProvider messageRouteProvider,
            IPipelineFactory pipelineFactory, IEndpointAggregator endpointAggregator,
            ISentinelObserver sentinelObserver, ISentinelConfiguration sentinelConfiguration)
        {
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(messageRouteProvider, nameof(messageRouteProvider));
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));
            Guard.AgainstNull(sentinelObserver, nameof(sentinelObserver));
            Guard.AgainstNull(sentinelConfiguration, nameof(sentinelConfiguration));

            _bus = bus;
            _messageRouteProvider = messageRouteProvider;
            _endpointAggregator = endpointAggregator;
            _sentinelObserver = sentinelObserver;
            _sentinelConfiguration = sentinelConfiguration;

            pipelineFactory.PipelineCreated += PipelineCreated;
        }

        public void Dispose()
        {
            _active = false;
        }

        private void PipelineCreated(object sender, PipelineEventArgs e)
        {
            var pipelineName = e.Pipeline.GetType().FullName ?? string.Empty;

            if (pipelineName.Equals(_startupPipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                e.Pipeline.RegisterObserver(this);

                return;
            }

            if (!pipelineName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase)
                &&
                !pipelineName.Equals(_dispatchTransportMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (pipelineName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
            {
                e.Pipeline.GetStage("Handle")
                    .BeforeEvent<OnHandleMessage>()
                    .Register<OnBeforeHandleMessage>();
            }

            e.Pipeline.RegisterObserver(_sentinelObserver);
        }

        public void Execute(OnStarted pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));
            
            if (!_messageRouteProvider.GetRouteUris(typeof(RegisterEndpointCommand).FullName).Any())
            {
                Log.For(this).Warning(Resources.WarningSentinelRouteMissing);
                return;
            }

            _cancellationToken = pipelineEvent.Pipeline.State.GetCancellationToken();

            _thread = new Thread(Send);

            _active = true;
            _thread.Start();

            while (!_thread.IsAlive)
            {
            }
        }

        private void Send()
        {
            while (_active)
            {
                if (_sentinelConfiguration.Enabled && _nextSendDate <= DateTime.Now)
                {
                    _bus.Send(_endpointAggregator.GetRegisterEndpointCommand());

                    _nextSendDate = DateTime.Now.Add(_sentinelConfiguration.HeartbeatIntervalDuration);
                }

                ThreadSleep.While(1000, _cancellationToken);
            }

            _thread.Join(TimeSpan.FromSeconds(5));
        }
    }
}