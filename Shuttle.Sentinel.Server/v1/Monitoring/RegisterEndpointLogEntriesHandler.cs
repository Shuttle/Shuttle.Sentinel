using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.LogEntry;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterEndpointLogEntriesHandler : EndpointMessageHandler, IMessageHandler<RegisterEndpointLogEntries>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly ILogEntryQuery _logEntryQuery;

        public RegisterEndpointLogEntriesHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery, ILogEntryQuery logEntryQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(logEntryQuery, nameof(logEntryQuery));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _logEntryQuery = logEntryQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterEndpointLogEntries> context)
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

                foreach (var logEntry in message.LogEntries)
                {
                    _logEntryQuery.Register(endpointId, logEntry.DateLogged, logEntry.Message, logEntry.LogLevel, logEntry.Category, logEntry.EventId, logEntry.Scope);
                }

                _endpointQuery.RegisterHeartbeat(endpointId);
            }
        }
    }
}