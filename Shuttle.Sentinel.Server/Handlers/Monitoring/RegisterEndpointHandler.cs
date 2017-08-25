using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterEndpointHandler : IMessageHandler<RegisterEndpointCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public RegisterEndpointHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterEndpointCommand> context)
        {
            var message = context.Message;

            if (string.IsNullOrEmpty(message.EndpointName)
                ||
                string.IsNullOrEmpty(message.MachineName)
                ||
                string.IsNullOrEmpty(message.BaseDirectory)
                ||
                string.IsNullOrEmpty(message.EntryAssemblyQualifiedName)
                ||
                string.IsNullOrEmpty(message.IPv4Address)
                ||
                string.IsNullOrEmpty(message.InboxWorkQueueUri))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                _endpointQuery.Register(
                    message.EndpointName,
                    message.MachineName,
                    message.BaseDirectory,
                    message.EntryAssemblyQualifiedName,
                    message.IPv4Address,
                    message.InboxWorkQueueUri,
                    message.ControlInboxWorkQueueUri);
            }
        }
    }
}