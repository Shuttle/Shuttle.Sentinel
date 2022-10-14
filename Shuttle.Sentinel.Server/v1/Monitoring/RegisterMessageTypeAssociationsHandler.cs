using System;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeAssociationsHandler : EndpointMessageHandler, IMessageHandler<RegisterMessageTypeAssociations>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly IMessageTypeAssociationQuery _messageTypeAssociationQuery;

        public RegisterMessageTypeAssociationsHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery, IMessageTypeAssociationQuery messageTypeAssociationQuery)
        {
            Guard.AgainstNull(messageTypeAssociationQuery, nameof(messageTypeAssociationQuery));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _messageTypeAssociationQuery = messageTypeAssociationQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMessageTypeAssociations> context)
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

                foreach (var association in message.MessageTypeAssociations)
                {
                    _messageTypeAssociationQuery.Register(
                        endpointId,
                        association.MessageTypeHandled,
                        association.MessageTypeDispatched);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}