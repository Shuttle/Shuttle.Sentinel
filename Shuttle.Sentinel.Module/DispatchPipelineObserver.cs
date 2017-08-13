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

        public void Execute(OnAfterDispatchTransportMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;

            var transportMessage = state.GetTransportMessage();
            var transportMessageReceived = state.GetTransportMessageReceived();
            var messageSender = state.GetHandlerContext() as IMessageSender;

            if (messageSender == null)
            {
                return;
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

                    messageSender.Send(
                        new RegisterMessageTypeAssociationCommand
                        {
                            MessageTypeHandled = transportMessageReceived.MessageType,
                            MessageTypeDispatched = transportMessage.MessageType
                        });
                }
                else
                {
                    if (_messageTypeDispatched.Contains(transportMessage.MessageType))
                    {
                        return;
                    }

                    _messageTypeDispatched.Add(transportMessage.MessageType);

                    messageSender.Send(
                        new RegisterMessageTypeDispatchedCommand {MessageType = transportMessage.MessageType});
                }
            }
        }
    }
}