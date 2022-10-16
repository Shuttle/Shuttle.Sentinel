using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Tag;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class EndpointStartedHandler : IMessageHandler<EndpointStarted>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly ITagQuery _tagQuery;
        private readonly ServerOptions _serverOptions;

        public EndpointStartedHandler(IOptions<ServerOptions> serverOptions, IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery, ITagQuery tagQuery)
        {
            Guard.AgainstNull(serverOptions, nameof(serverOptions));
            Guard.AgainstNull(serverOptions.Value, nameof(serverOptions.Value));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(tagQuery, nameof(tagQuery));

            _serverOptions = serverOptions.Value;
            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _tagQuery = tagQuery;
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
                    message.EnvironmentName,
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

            if (message.Subscriptions.Any())
            {
                context.Send(new RegisterSubscriptions
                {
                    MachineName = message.MachineName,
                    BaseDirectory = message.BaseDirectory,
                    MessageTypes = message.Subscriptions
                }, builder => builder.Local());
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
            }, builder => builder.Local());
        }
    }
}