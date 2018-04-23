using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
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

            if (string.IsNullOrEmpty(message.MachineName)
                ||
                string.IsNullOrEmpty(message.BaseDirectory))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                _endpointQuery.Save(
                    message.MachineName,
                    message.BaseDirectory,
                    message.EntryAssemblyQualifiedName,
                    message.IPv4Address,
                    message.InboxWorkQueueUri,
                    message.InboxDeferredQueueUri,
                    message.InboxErrorQueueUri,
                    message.OutboxWorkQueueUri,
                    message.OutboxErrorQueueUri,
                    message.ControlInboxWorkQueueUri,
                    message.ControlInboxErrorQueueUri);

                var id = _endpointQuery.FindId(message.MachineName, message.BaseDirectory);

                if (!id.HasValue)
                {
                    return;
                }

                var endpointId = id.Value;

                foreach (var metric in message.MessageTypeMetrics)
                {
                    _endpointQuery.AddMessageTypeMetric(
                        endpointId,
                        metric.MessageType,
                        metric.Count,
                        metric.FastestExecutionDuration,
                        metric.SlowestExecutionDuration,
                        metric.TotalExecutionDuration);
                }

                foreach (var association in message.MessageTypeAssociations)
                {
                    _endpointQuery.AddMessageTypeAssociation(
                        endpointId,
                        association.MessageTypeHandled,
                        association.MessageTypeDispatched);
                }

                foreach (var dispatched in message.MessageTypesDispatched)
                {
                    _endpointQuery.AddMessageTypeDispatched(
                        endpointId,
                        dispatched.MessageType,
                        dispatched.RecipientInboxWorkQueueUri);
                }

                foreach (var messageType in message.MessageTypesHandled)
                {
                    _endpointQuery.AddMessageTypeHandled(
                        endpointId,
                        messageType);
                }
            }

            if (!string.IsNullOrEmpty(message.InboxWorkQueueUri))
            {
                context.Send(new SaveQueueCommand
                {
                    QueueUri = message.InboxWorkQueueUri,
                    Processor = "inbox",
                    Type = "work"
                }, c => c.Local());
            }
        }
    }
}