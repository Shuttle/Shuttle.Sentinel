using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterHeartbeatHandler : IMessageHandler<RegisterHeartbeat>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public RegisterHeartbeatHandler(IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterHeartbeat> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                var id = _endpointQuery.FindId(message.MachineName, message.BaseDirectory);

                if (!id.HasValue)
                {
                    return;
                }

                _endpointQuery.RegisterHeartbeat(id.Value);
            }
        }
    }
}