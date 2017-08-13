using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class InboxPipelineObserver :
        IPipelineObserver<OnHandleMessage>,
        IPipelineObserver<OnAfterHandleMessage>
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _messageTypeHandled = new HashSet<string>();

        private readonly IMetricCollector _metricCollector;

        private const string Key = "[InboxPipelineObserver/StartDate]";

        public InboxPipelineObserver(IMetricCollector metricCollector)
        {
            Guard.AgainstNull(metricCollector, nameof(metricCollector));

            _metricCollector = metricCollector;
        }

        public void Execute(OnAfterHandleMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();

            if (transportMessage == null)
            {
                return;
            }

            try
            {
                var duration = (DateTime.Now - state.Get<DateTime>(Key)).TotalMilliseconds;

                _metricCollector.AddExecutionDuration(transportMessage.MessageType, duration);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }

            var messageSender = state.GetHandlerContext() as IMessageSender;

            if (messageSender == null)
            {
                return;
            }

            lock (_lock)
            {
                if (_messageTypeHandled.Contains(transportMessage.MessageType))
                {
                    return;
                }

                _messageTypeHandled.Add(transportMessage.MessageType);

                messageSender.Send(
                    new RegisterMessageTypeHandledCommand { MessageType = transportMessage.MessageType });
            }
        }

        public void Execute(OnHandleMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;

            state.Add(Key, DateTime.Now);
        }
    }
}