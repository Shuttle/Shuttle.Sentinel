using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypesHandledHandler : EndpointMessageHandler, IMessageHandler<RegisterMessageTypesHandled>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly IMessageTypeHandledQuery _messageTypeHandledQuery;

        public RegisterMessageTypesHandledHandler(IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery, IMessageTypeHandledQuery messageTypeHandledQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(messageTypeHandledQuery, nameof(messageTypeHandledQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _messageTypeHandledQuery = messageTypeHandledQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterMessageTypesHandled> context)
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

                foreach (var messageType in message.MessageTypesHandled)
                {
                    _messageTypeHandledQuery.Register(
                        endpointId,
                        messageType);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}