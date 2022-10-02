using System;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class EndpointStartedHandler : IMessageHandler<EndpointStarted>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly ServerOptions _serverOptions;

        public EndpointStartedHandler(IOptions<ServerOptions> serverOptions, IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(serverOptions, nameof(serverOptions));
            Guard.AgainstNull(serverOptions.Value, nameof(serverOptions.Value));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));

            _serverOptions = serverOptions.Value;
            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
        }

        public void ProcessMessage(IHandlerContext<EndpointStarted> context)
        {
            var message = context.Message;

            if (string.IsNullOrEmpty(message.MachineName)
                ||
                string.IsNullOrEmpty(message.BaseDirectory)
                ||
                context.TransportMessage.SendDate < DateTime.UtcNow.Subtract(_serverOptions.HeartbeatIntervalDuration))
            {
                return;
            }

            string heartbeatIntervalDuration;

            try
            {
                var span = TimeSpan.Parse(message.HeartbeatIntervalDuration);

                heartbeatIntervalDuration = span.ToString();
            }
            catch
            {
                heartbeatIntervalDuration = _serverOptions.HeartbeatIntervalDuration.ToString();
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
                    message.ControlInboxErrorQueueUri,
                    message.TransientInstance,
                    heartbeatIntervalDuration);
            }

            RegisterQueue(context, message.InboxWorkQueueUri, "inbox", "work");
            RegisterQueue(context, message.InboxDeferredQueueUri, "inbox", "deferred");
            RegisterQueue(context, message.InboxErrorQueueUri, "inbox", "error");
            RegisterQueue(context, message.OutboxWorkQueueUri, "outbox", "work");
            RegisterQueue(context, message.OutboxErrorQueueUri, "outbox", "error");
            RegisterQueue(context, message.ControlInboxWorkQueueUri, "control-inbox", "work");
            RegisterQueue(context, message.ControlInboxErrorQueueUri, "control-inbox", "error");
        }

        private void RegisterQueue(IHandlerContext context, string uri, string processor,
            string type)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            context.Send(new RegisterQueue
            {
                Uri = uri,
                Processor = processor,
                Type = type
            }, c => c.Local());
        }
    }
}