using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMetricsHandler : IMessageHandler<RegisterMetricsCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public RegisterMetricsHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMetricsCommand> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                var id = _endpointQuery.FindId(message.EndpointName, message.MachineName, message.BaseDirectory);

                if (!id.HasValue)
                {
                    return;
                }

                var endpointId = id.Value;

                foreach (var metric in message.MessageTypeMetrics)
                {
                    _endpointQuery.AddMessageTypeMetric(
                        endpointId,
                        metric.MessageType,
                        metric.Count,
                        metric.FastestExecutionDuration,
                        metric.SlowestExecutionDuration,
                        metric.TotalExecutionDuration);
                }

                foreach (var association in message.MessageTypeAssociations)
                {
                    _endpointQuery.AddMessageTypeAssociation(
                        endpointId, 
                        association.MessageTypeHandled,
                        association.MessageTypeDispatched);
                }

                foreach (var dispatched in message.MessageTypesDispatched)
                {
                    _endpointQuery.AddMessageTypeDispatched(
                        endpointId,
                        dispatched.MessageType,
                        dispatched.RecipientInboxWorkQueueUri);
                }

                foreach (var messageType in message.MessageTypesHandled)
                {
                    _endpointQuery.AddMessageTypeHandled(
                        endpointId,
                        messageType);
                }

                //foreach (var systemMetric in message.SystemMetrics)
                //{
                //    _endpointQuery.AddSystemMetric()
                //}
            }
        }
    }
}