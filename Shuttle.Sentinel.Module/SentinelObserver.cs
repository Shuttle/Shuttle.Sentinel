using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class SentinelObserver : ISentinelObserver
    {
        private readonly IEndpointAggregator _endpointAggregator;

        public SentinelObserver(IEndpointAggregator endpointAggregator)
        {
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));

            _endpointAggregator = endpointAggregator;
        }

        public void Execute(OnBeforeHandleMessage pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var processingStatus = state.GetProcessingStatus();

            if (processingStatus == ProcessingStatus.Ignore || processingStatus == ProcessingStatus.MessageHandled)
            {
                return;
            }

            var transportMessage = state.GetTransportMessage();

            if (transportMessage.HasExpired())
            {
                return;
            }

            _endpointAggregator.MessageProcessingStart(transportMessage.MessageId);
        }

        public void Execute(OnAfterHandleMessage pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();
            var message = state.GetMessage();

            _endpointAggregator.MessageProcessingEnd(transportMessage.MessageId, message.GetType());
        }

        public void Execute(OnPipelineException pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();

            if (transportMessage == null)
            {
                return;
            }

            _endpointAggregator.MessageProcessingAborted(transportMessage.MessageId);
        }

        public void Execute(OnAfterDispatchTransportMessage pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();
            var transportMessageReceived = state.GetTransportMessageReceived();

            if (transportMessageReceived != null)
            {
                _endpointAggregator.RegisterAssociation(transportMessageReceived.MessageType,
                    transportMessage.MessageType);
            }

            _endpointAggregator.RegisterDispatched(transportMessage.MessageType,
                transportMessage.RecipientInboxWorkQueueUri);
        }
    }
}