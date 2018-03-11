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
                var id = _serverQuery.FindId(message.MachineName, message.BaseDirectory);

                if (id.HasValue)
                {
                    _serverQuery.Save(
                        id.Value, 
                        message.IPv4Address, 
                        message.InboxWorkQueueUri,
                        message.ControlInboxWorkQueueUri);
                }
                else
                {
                    _serverQuery.Add(
                        message.MachineName, 
                        message.BaseDirectory,
                        message.IPv4Address,
                        message.InboxWorkQueueUri,
                        message.ControlInboxWorkQueueUri);
                }
            }
        }
    }
}