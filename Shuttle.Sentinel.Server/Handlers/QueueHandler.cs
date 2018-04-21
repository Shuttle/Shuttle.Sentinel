using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class QueueHandler : 
        IMessageHandler<SaveQueueCommand>,
        IMessageHandler<RemoveQueueCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IQueueQuery _queueQuery;

        public QueueHandler(IDatabaseContextFactory databaseContextFactory, IQueueQuery queueQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(queueQuery, nameof(queueQuery));

            _databaseContextFactory = databaseContextFactory;
            _queueQuery = queueQuery;
        }

        public void ProcessMessage(IHandlerContext<SaveQueueCommand> context)
        {
            Uri uri;

            var message = context.Message;

            try
            {
                uri = new Uri(message.QueueUri);
            }
            catch
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                _queueQuery.Save(message.QueueUri, message.Processor, message.Type);
            }
        }

        public void ProcessMessage(IHandlerContext<RemoveQueueCommand> context)
        {
            using (_databaseContextFactory.Create())
            {
                _queueQuery.Remove(context.Message.Id);
            }
        }
    }
}