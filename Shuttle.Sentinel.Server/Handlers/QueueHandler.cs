using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class QueueHandler : 
        IMessageHandler<RegisterQueueCommand>,
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

        public void ProcessMessage(IHandlerContext<RegisterQueueCommand> context)
        {
            Uri uri;

            var message = context.Message;

            try
            {
                uri = new Uri(message.Uri);
            }
            catch
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                _queueQuery.Save(message.Uri, message.Processor, message.Type);
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