using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;
using Shuttle.Sentinel.Module;

namespace Shuttle.Sentinel.Server
{
    public class RegisterEndpointHandler : IMessageHandler<RegisterEndpointCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly ISentinelConfiguration _configuration;

        public RegisterEndpointHandler(IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery,
            ISentinelConfiguration configuration)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(configuration, nameof(configuration));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _configuration = configuration;
        }

        public void ProcessMessage(IHandlerContext<RegisterEndpointCommand> context)
        {
            var message = context.Message;

            if (string.IsNullOrEmpty(message.MachineName)
                ||
                string.IsNullOrEmpty(message.BaseDirectory)
                ||
                context.TransportMessage.SendDate < DateTime.Now.Subtract(_configuration.HeartbeatIntervalDuration))
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
                        context.TransportMessage.MessageId,
                        metric.MessageType,
                        context.TransportMessage.SendDate,
                        endpointId,
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

            SaveQueue(context, message.InboxWorkQueueUri, "inbox", "work");
            SaveQueue(context, message.InboxDeferredQueueUri, "inbox", "deferred");
            SaveQueue(context, message.InboxErrorQueueUri, "inbox", "error");
            SaveQueue(context, message.OutboxWorkQueueUri, "outbox", "work");
            SaveQueue(context, message.OutboxErrorQueueUri, "outbox", "error");
            SaveQueue(context, message.ControlInboxWorkQueueUri, "control-inbox", "work");
            SaveQueue(context, message.ControlInboxErrorQueueUri, "control-inbox", "error");
        }

        private void SaveQueue(IHandlerContext<RegisterEndpointCommand> context, string uri, string processor,
            string type)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            context.Send(new SaveQueueCommand
            {
                QueueUri = uri,
                Processor = processor,
                Type = type
            }, c => c.Local());
        }
    }
}