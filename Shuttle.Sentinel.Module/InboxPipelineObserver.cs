using System;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class InboxPipelineObserver :
        IPipelineObserver<OnHandleMessage>,
        IPipelineObserver<OnAfterHandleMessage>
    {
        private const string Key = "[InboxPipelineObserver/StartDate]";

        private readonly IMetricCollector _metricCollector;

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
        }

        public void Execute(OnHandleMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;

            state.Replace(Key, DateTime.Now);
        }
    }
}