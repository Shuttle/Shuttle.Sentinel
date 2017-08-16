using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class DispatchPipelineObserver : IPipelineObserver<OnAfterDispatchTransportMessage>
    {
        private static readonly object _lock = new object();

        private static readonly HashSet<string> _messageTypeAssociation = new HashSet<string>();
        private static readonly HashSet<string> _messageTypeDispatched = new HashSet<string>();
        private readonly IServiceBus _bus;
        private readonly ISentinelConfiguration _configuration;

        public DispatchPipelineObserver(IServiceBus bus, ISentinelConfiguration configuration)
        {
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(configuration, nameof(configuration));

            _bus = bus;
            _configuration = configuration;
        }

        public void Execute(OnAfterDispatchTransportMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;

            var transportMessage = state.GetTransportMessage();
            var transportMessageReceived = state.GetTransportMessageReceived();

            if (transportMessageReceived != null)
            {
                if (_messageTypeAssociation.Contains($"{transportMessage.MessageType}/{transportMessageReceived.MessageType}"))
                {
                    return;
                }
            }
            else
            {
                if (_messageTypeDispatched.Contains(transportMessage.MessageType))
                {
                    return;
                }
            }

            lock (_lock)
            {
                if (transportMessageReceived != null)
                {
                    var key = $"{transportMessage.MessageType}/{transportMessageReceived.MessageType}";

                    if (_messageTypeAssociation.Contains(key))
                    {
                        return;
                    }

                    _messageTypeAssociation.Add(key);

                    _bus.Send(
                        new RegisterMessageTypeAssociationCommand
                        {
                            MessageTypeHandled = transportMessageReceived.MessageType,
                            MessageTypeDispatched = transportMessage.MessageType
                        }, c => c.WithRecipient(_configuration.InboxWorkQueueUri));
                }
                else
                {
                    if (_messageTypeDispatched.Contains(transportMessage.MessageType))
                    {
                        return;
                    }

                    _messageTypeDispatched.Add(transportMessage.MessageType);

                    _bus.Send(
                        new RegisterMessageTypeDispatchedCommand
                        {
                            MessageType = transportMessage.MessageType
                        }, c => c.WithRecipient(_configuration.InboxWorkQueueUri));
                }
            }
        }
    }
}