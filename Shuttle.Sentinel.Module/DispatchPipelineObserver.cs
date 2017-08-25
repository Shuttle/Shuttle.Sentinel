using System;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class DispatchPipelineObserver : IPipelineObserver<OnAfterDispatchTransportMessage>
    {
        private readonly IMetricCollector _metricCollector;

        public DispatchPipelineObserver(IMetricCollector metricCollector)
        {
            Guard.AgainstNull(metricCollector, nameof(metricCollector));

            _metricCollector = metricCollector;
        }

        public void Execute(OnAfterDispatchTransportMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;

            var transportMessage = state.GetTransportMessage();
            var transportMessageReceived = state.GetTransportMessageReceived();

            if (
                transportMessageReceived != null
                &&
                !transportMessageReceived.MessageType.Equals(
                    transportMessage.MessageType, StringComparison.InvariantCultureIgnoreCase) // ignore forwarding
            )
            {
                _metricCollector.AddMessageTypeAssociation(
                    transportMessageReceived.MessageType,
                    transportMessage.MessageType);
            }

            _metricCollector.AddMessageTypeDispatched(transportMessage.MessageType,
                transportMessage.RecipientInboxWorkQueueUri);
        }
    }
}