using System;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypesDispatchedHandler : EndpointMessageHandler, IMessageHandler<RegisterMessageTypesDispatched>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly IMessageTypeDispatchedQuery _messageTypeDispatchedQuery;

        public RegisterMessageTypesDispatchedHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery, IMessageTypeDispatchedQuery messageTypeDispatchedQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(messageTypeDispatchedQuery, nameof(messageTypeDispatchedQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _messageTypeDispatchedQuery = messageTypeDispatchedQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMessageTypesDispatched> context)
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

                foreach (var dispatched in message.MessageTypesDispatched)
                {
                    _messageTypeDispatchedQuery.Register(
                        endpointId,
                        dispatched.MessageType,
                        dispatched.RecipientInboxWorkQueueUri);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}