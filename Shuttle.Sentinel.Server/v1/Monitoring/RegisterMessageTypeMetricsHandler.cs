using System;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeMetricsHandler : EndpointMessageHandler, IMessageHandler<RegisterMessageTypeMetrics>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly IMessageTypeMetricQuery _messageTypeMetricQuery;

        public RegisterMessageTypeMetricsHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery, IMessageTypeMetricQuery messageTypeMetricQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(messageTypeMetricQuery, nameof(messageTypeMetricQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _messageTypeMetricQuery = messageTypeMetricQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMessageTypeMetrics> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                var id = _endpointQuery.FindId(message.MachineName, message.BaseDirectory);

                if (!id.HasValue)
                {
                    Defer(context, message);

                    return;
                }

                var endpointId = id.Value;

                foreach (var metric in message.MessageTypeMetrics)
                {
                    _messageTypeMetricQuery.Register(
                        context.TransportMessage.MessageId,
                        metric.MessageType,
                        context.TransportMessage.SendDate,
                        endpointId,
                        metric.Count,
                        metric.FastestExecutionDuration,
                        metric.SlowestExecutionDuration,
                        metric.TotalExecutionDuration);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}