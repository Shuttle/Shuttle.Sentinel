using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IDisposable, IPipelineObserver<OnStarted>
    {
        private readonly Type _inboxMessagePipelineType = typeof(InboxMessagePipeline);
        private readonly Type _dispatchTransportMessagePipelineType = typeof(DispatchTransportMessagePipeline);
        private readonly Type _startupPipelineType = typeof(StartupPipeline);

        private readonly IServiceBus _serviceBus;
        private readonly IMessageRouteProvider _messageRouteProvider;
        private readonly IEndpointAggregator _endpointAggregator;
        private readonly ISentinelObserver _sentinelObserver;
        private volatile bool _active;
        private Thread _thread;
        private DateTime _nextSendDate = DateTime.Now;
        private CancellationToken _cancellationToken;
        private readonly SentinelOptions _sentinelOptions;

        public SentinelModule(IOptions<SentinelOptions> sentinelOptions, IServiceBus serviceBus, IMessageRouteProvider messageRouteProvider,
            IPipelineFactory pipelineFactory, IEndpointAggregator endpointAggregator,
            ISentinelObserver sentinelObserver)
        {
            Guard.AgainstNull(sentinelOptions, nameof(sentinelOptions));
            Guard.AgainstNull(sentinelOptions.Value, nameof(sentinelOptions.Value));
            Guard.AgainstNull(serviceBus, nameof(serviceBus));
            Guard.AgainstNull(messageRouteProvider, nameof(messageRouteProvider));
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));
            Guard.AgainstNull(sentinelObserver, nameof(sentinelObserver));

            _sentinelOptions = sentinelOptions.Value;
            _serviceBus = serviceBus;
            _messageRouteProvider = messageRouteProvider;
            _endpointAggregator = endpointAggregator;
            _sentinelObserver = sentinelObserver;

            pipelineFactory.PipelineCreated += PipelineCreated;
        }

        public void Dispose()
        {
            _active = false;
        }

        private void PipelineCreated(object sender, PipelineEventArgs e)
        {
            var pipelineType = e.Pipeline.GetType();

            if (pipelineType == _startupPipelineType)
            {
                e.Pipeline.RegisterObserver(this);

                return;
            }

            if (pipelineType != _inboxMessagePipelineType
                &&
                pipelineType != _dispatchTransportMessagePipelineType)
            {
                return;
            }

            if (pipelineType == _inboxMessagePipelineType)
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
            
            if (!_messageRouteProvider.GetRouteUris(typeof(RegisterEndpoint).FullName).Any())
            {
                RouteMissing.Invoke(this, new RouteMissingEventArgs(typeof(RegisterEndpoint)));
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
                if (_sentinelOptions.Enabled && _nextSendDate <= DateTime.Now)
                {
                    _serviceBus.Send(_endpointAggregator.GetRegisterEndpointCommand());

                    _nextSendDate = DateTime.Now.Add(_sentinelOptions.HeartbeatIntervalDuration);
                }

                Task.Delay(1000, _cancellationToken).Wait(_cancellationToken);
            }

            _thread.Join(TimeSpan.FromSeconds(5));
        }

        public event EventHandler<RouteMissingEventArgs> RouteMissing = delegate
        {
        };

    }
}