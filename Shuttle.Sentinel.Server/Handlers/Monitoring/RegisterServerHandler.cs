using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterServerHandler : IMessageHandler<RegisterServerCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IServerQuery _serverQuery;

        public RegisterServerHandler(IDatabaseContextFactory databaseContextFactory, IServerQuery serverQuery)
        {
            Guard.AgainstNull(databaseContextFactory,nameof(databaseContextFactory));
            Guard.AgainstNull(serverQuery, nameof(serverQuery));

            _databaseContextFactory = databaseContextFactory;
            _serverQuery = serverQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterServerCommand> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                    _serverQuery.Save(
                        message.MachineName,
                        message.BaseDirectory,
                        message.IPv4Address, 
                        message.InboxWorkQueueUri,
                        message.InboxDeferredQueueUri,
                        message.InboxErrorQueueUri,
                        message.OutboxWorkQueueUri,
                        message.OutboxErrorQueueUri,
                        message.ControlInboxWorkQueueUri,
                        message.ControlInboxErrorQueueUri);
            }
        }
    }
}