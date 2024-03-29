﻿using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterSystemMetricsHandler : EndpointMessageHandler, IMessageHandler<RegisterSystemMetrics>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public RegisterSystemMetricsHandler(IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterSystemMetrics> context)
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

                foreach (var systemMetric in message.SystemMetrics)
                {
                    _endpointQuery.RegisterSystemMetric(endpointId, systemMetric.DateRegistered , systemMetric.Name, systemMetric.Value);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}