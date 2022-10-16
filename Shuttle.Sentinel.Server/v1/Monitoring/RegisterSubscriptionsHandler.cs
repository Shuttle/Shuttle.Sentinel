using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterSubscriptionsHandler : EndpointMessageHandler, IMessageHandler<RegisterSubscriptions>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly ISubscriptionQuery _subscriptionQuery;

        public RegisterSubscriptionsHandler(IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery, ISubscriptionQuery subscriptionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(subscriptionQuery, nameof(subscriptionQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _subscriptionQuery = subscriptionQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterSubscriptions> context)
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

                foreach (var messageType in message.MessageTypes)
                {
                    _subscriptionQuery.Register(endpointId, messageType);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}