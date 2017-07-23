using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel.Server
{
    public class SubscriptionHandler : 
        IMessageHandler<AddSubscriptionCommand>,
        IMessageHandler<RemoveSubscriptionCommand>
    {
        private readonly IDataStoreDatabaseContextFactory _databaseContextFactory;
        private readonly ISubscriptionQuery _subscriptionQuery;

        public SubscriptionHandler(IDataStoreDatabaseContextFactory databaseContextFactory, ISubscriptionQuery subscriptionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(subscriptionQuery, "subscriptionQuery");

            _databaseContextFactory = databaseContextFactory;
            _subscriptionQuery = subscriptionQuery;
        }

        public void ProcessMessage(IHandlerContext<AddSubscriptionCommand> context)
        {
            Guard.AgainstNull(context,"context");

            var message = context.Message;

            using (_databaseContextFactory.Create(message.DataStoreId))
            {
                _subscriptionQuery.Add(new Subscription
                {
                    MessageType = message.MessageType,
                    InboxWorkQueueUri = message.InboxWorkQueueUri
                });
            }
        }

        public void ProcessMessage(IHandlerContext<RemoveSubscriptionCommand> context)
        {
            Guard.AgainstNull(context, "context");

            var message = context.Message;

            using (_databaseContextFactory.Create(message.DataStoreId))
            {
                _subscriptionQuery.Remove(new Subscription
                {
                    MessageType = message.MessageType,
                    InboxWorkQueueUri = message.InboxWorkQueueUri
                });
            }
        }
    }
}