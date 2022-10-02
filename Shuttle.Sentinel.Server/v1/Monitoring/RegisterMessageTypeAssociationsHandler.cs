using System;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeAssociationsHandler : IMessageHandler<RegisterMessageTypeAssociations>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public RegisterMessageTypeAssociationsHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMessageTypeAssociations> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                var id = _endpointQuery.FindId(message.MachineName, message.BaseDirectory);

                if (!id.HasValue)
                {
                    return;
                }

                var endpointId = id.Value;

                foreach (var association in message.MessageTypeAssociations)
                {
                    _endpointQuery.AddMessageTypeAssociation(
                        endpointId,
                        association.MessageTypeHandled,
                        association.MessageTypeDispatched);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}