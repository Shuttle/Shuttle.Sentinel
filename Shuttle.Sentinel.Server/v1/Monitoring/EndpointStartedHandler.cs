using System;
using System.Collections.Generic;
using System.Net.Http;
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
                _endpointQuery.Started(
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
                    heartbeatIntervalDuration,
                    context.TransportMessage.SendDate);
            }

            RegisterQueue(context, message.InboxWorkQueueUri, "inbox", "work", message.Tags);
            RegisterQueue(context, message.InboxDeferredQueueUri, "inbox", "deferred", message.Tags);
            RegisterQueue(context, message.InboxErrorQueueUri, "inbox", "error", message.Tags);
            RegisterQueue(context, message.OutboxWorkQueueUri, "outbox", "work", message.Tags);
            RegisterQueue(context, message.OutboxErrorQueueUri, "outbox", "error", message.Tags);
            RegisterQueue(context, message.ControlInboxWorkQueueUri, "control-inbox", "work", message.Tags);
            RegisterQueue(context, message.ControlInboxErrorQueueUri, "control-inbox", "error", message.Tags);
        }

        private void RegisterQueue(IHandlerContext context, string uri, string processor,
            string type, List<string> tags)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            context.Send(new RegisterQueue
            {
                Uri = uri,
                Processor = processor,
                Type = type,
                Tags = tags
            }, c => c.Local());
        }
    }
}