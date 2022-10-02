using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;
using System.Reflection;

namespace Shuttle.Sentinel.Module
{
    public class SentinelModule : IDisposable, IPipelineObserver<OnStarted>, IPipelineObserver<OnStopping>
    {
        private readonly Type _inboxMessagePipelineType = typeof(InboxMessagePipeline);
        private readonly Type _dispatchTransportMessagePipelineType = typeof(DispatchTransportMessagePipeline);
        private readonly Type _startupPipelineType = typeof(StartupPipeline);
        private readonly Type _shutdownPipelineType = typeof(ShutdownPipeline);

        private readonly IServiceBus _serviceBus;
        private readonly IMessageRouteProvider _messageRouteProvider;
        private readonly IEndpointAggregator _endpointAggregator;
        private readonly ISentinelObserver _sentinelObserver;
        private volatile bool _active;
        private Thread _thread;
        private DateTime _nextSendDate = DateTime.UtcNow;
        private CancellationToken _cancellationToken;
        private readonly SentinelOptions _sentinelOptions;
        private readonly ServiceBusOptions _serviceBusOptions;

        public SentinelModule(IOptions<SentinelOptions> sentinelOptions, IOptions<ServiceBusOptions> serviceBusOptions, IServiceBus serviceBus, IMessageRouteProvider messageRouteProvider,
            IPipelineFactory pipelineFactory, IEndpointAggregator endpointAggregator,
            ISentinelObserver sentinelObserver)
        {
            Guard.AgainstNull(sentinelOptions, nameof(sentinelOptions));
            Guard.AgainstNull(sentinelOptions.Value, nameof(sentinelOptions.Value));
            Guard.AgainstNull(serviceBusOptions, nameof(serviceBusOptions));
            Guard.AgainstNull(serviceBusOptions.Value, nameof(serviceBusOptions.Value));
            Guard.AgainstNull(serviceBus, nameof(serviceBus));
            Guard.AgainstNull(messageRouteProvider, nameof(messageRouteProvider));
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));
            Guard.AgainstNull(sentinelObserver, nameof(sentinelObserver));

            _serviceBusOptions = serviceBusOptions.Value;
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

            if (pipelineType == _startupPipelineType ||
                pipelineType == _shutdownPipelineType)
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
            
            if (!_messageRouteProvider.GetRouteUris(typeof(EndpointStarted).FullName).Any())
            {
                RouteMissing.Invoke(this, new RouteMissingEventArgs(typeof(EndpointStarted)));
                return;
            }

            _cancellationToken = pipelineEvent.Pipeline.State.GetCancellationToken();

            var ipv4Address = "0.0.0.0";

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                ipv4Address = ip.ToString();
            }

            _serviceBus.Send(new EndpointStarted
            {
                MachineName = Environment.MachineName,
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory,
                IPv4Address = ipv4Address,
                EntryAssemblyQualifiedName = Assembly.GetEntryAssembly()?.FullName ?? "(unknown)",
                InboxWorkQueueUri = _serviceBusOptions.Inbox?.WorkQueueUri ?? string.Empty,
                InboxDeferredQueueUri = _serviceBusOptions.Inbox?.DeferredQueueUri ?? string.Empty,
                InboxErrorQueueUri = _serviceBusOptions.Inbox?.ErrorQueueUri ?? string.Empty,
                OutboxWorkQueueUri = _serviceBusOptions.Outbox?.WorkQueueUri ?? string.Empty,
                OutboxErrorQueueUri = _serviceBusOptions.Outbox?.ErrorQueueUri ?? string.Empty,
                ControlInboxWorkQueueUri = _serviceBusOptions.ControlInbox?.WorkQueueUri ?? string.Empty,
                ControlInboxErrorQueueUri = _serviceBusOptions.ControlInbox?.ErrorQueueUri ?? string.Empty,
                Subscriptions = _serviceBusOptions.Subscription.MessageTypes,
                TransientInstance = _sentinelOptions.TransientInstance
            });

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
                if (_sentinelOptions.Enabled && _nextSendDate <= DateTime.UtcNow)
                {
                    foreach (var command in _endpointAggregator.GetCommands())
                    {
                        if (!_messageRouteProvider.GetRouteUris(command.GetType().FullName).Any())
                        {
                            RouteMissing.Invoke(this, new RouteMissingEventArgs(command.GetType()));
                            continue;
                        }

                        _serviceBus.Send(command);
                    }

                    _nextSendDate = DateTime.UtcNow.Add(_sentinelOptions.HeartbeatIntervalDuration);
                }

                Task.Delay(1000, _cancellationToken).Wait(_cancellationToken);
            }

            _thread.Join(TimeSpan.FromSeconds(5));
        }

        public event EventHandler<RouteMissingEventArgs> RouteMissing = delegate
        {
        };

        public void Execute(OnStopping pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            if (!_messageRouteProvider.GetRouteUris(typeof(EndpointStopped).FullName).Any())
            {
                RouteMissing.Invoke(this, new RouteMissingEventArgs(typeof(EndpointStopped)));
                return;
            }

            _serviceBus.Send(new EndpointStopped
            {
                MachineName = Environment.MachineName,
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory
            });
        }
    }
}