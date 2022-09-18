using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class EndpointAggregator : IEndpointAggregator
    {
        private readonly List<RegisterEndpoint.Association> _associations =
            new List<RegisterEndpoint.Association>();

        private readonly List<RegisterEndpoint.Dispatched> _dispatched =
            new List<RegisterEndpoint.Dispatched>();

        private readonly string _ipv4Address;
        private readonly object _lock = new object();
        private readonly Dictionary<Guid, DateTime> _messageProcessingStartDates = new Dictionary<Guid, DateTime>();

        private readonly Dictionary<Type, RegisterEndpoint.MessageTypeMetric> _messageTypeMetrics =
            new Dictionary<Type, RegisterEndpoint.MessageTypeMetric>();

        private readonly List<string> _messageTypes = new List<string>();
        private readonly HashSet<string> _registeredAssociations = new HashSet<string>();
        private readonly HashSet<string> _registeredDispatched = new HashSet<string>();
        private readonly HashSet<string> _registeredMessageTypes = new HashSet<string>();
        private readonly string _entryAssemblyQualifiedName;
        private readonly ServiceBusOptions _serviceBusOptions;

        public EndpointAggregator(IOptions<ServiceBusOptions> serviceBusOptions)
        {
            Guard.AgainstNull(serviceBusOptions, nameof(serviceBusOptions));
            Guard.AgainstNull(serviceBusOptions.Value, nameof(serviceBusOptions.Value));

            _serviceBusOptions = serviceBusOptions.Value;

            _ipv4Address = "0.0.0.0";

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                _ipv4Address = ip.ToString();
            }

            _entryAssemblyQualifiedName = Assembly.GetEntryAssembly().FullName;
        }

        public void MessageProcessingStart(Guid messageId)
        {
            lock (_lock)
            {
                if (_messageProcessingStartDates.ContainsKey(messageId))
                {
                    _messageProcessingStartDates.Remove(messageId);
                }

                _messageProcessingStartDates.Add(messageId, DateTime.Now);
            }
        }

        public void MessageProcessingEnd(Guid messageId, Type messageType)
        {
            Guard.AgainstNull(messageType, nameof(messageType));

            lock (_lock)
            {
                if (!_registeredMessageTypes.Contains(messageType.FullName))
                {
                    _messageTypes.Add(messageType.FullName);
                    _registeredMessageTypes.Add(messageType.FullName);
                }

                if (!_messageProcessingStartDates.TryGetValue(messageId, out var startDate))
                {
                    return;
                }

                var duration = (DateTime.Now - startDate).TotalMilliseconds;

                if (!_messageTypeMetrics.TryGetValue(messageType, out var messageTypeMetric))
                {
                    messageTypeMetric = new RegisterEndpoint.MessageTypeMetric
                    {
                        MessageType = messageType.FullName,
                        FastestExecutionDuration = double.MaxValue,
                        SlowestExecutionDuration = 0,
                        Count = 0,
                        TotalExecutionDuration = 0
                    };

                    _messageTypeMetrics.Add(messageType, messageTypeMetric);
                }

                messageTypeMetric.Count = messageTypeMetric.Count + 1;

                if (messageTypeMetric.FastestExecutionDuration > duration)
                {
                    messageTypeMetric.FastestExecutionDuration = duration;
                }

                if (messageTypeMetric.SlowestExecutionDuration < duration)
                {
                    messageTypeMetric.SlowestExecutionDuration = duration;
                }

                messageTypeMetric.TotalExecutionDuration = messageTypeMetric.TotalExecutionDuration + duration;

                _messageProcessingStartDates.Remove(messageId);
            }
        }

        public void MessageProcessingAborted(Guid messageId)
        {
            lock (_lock)
            {
                _messageProcessingStartDates.Remove(messageId);
            }
        }

        public RegisterEndpoint GetRegisterEndpointCommand()
        {
            lock (_lock)
            {
                var result = new RegisterEndpoint
                {
                    MachineName = Environment.MachineName,
                    IPv4Address = _ipv4Address,
                    BaseDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    EntryAssemblyQualifiedName = _entryAssemblyQualifiedName,
                    InboxWorkQueueUri = _serviceBusOptions.Inbox?.WorkQueueUri ?? string.Empty,
                    InboxDeferredQueueUri = _serviceBusOptions.Inbox?.DeferredQueueUri ?? string.Empty,
                    InboxErrorQueueUri = _serviceBusOptions.Inbox?.ErrorQueueUri ?? string.Empty,
                    OutboxWorkQueueUri = _serviceBusOptions.Outbox?.WorkQueueUri ?? string.Empty,
                    OutboxErrorQueueUri = _serviceBusOptions.Outbox?.ErrorQueueUri ?? string.Empty,
                    ControlInboxWorkQueueUri = _serviceBusOptions.ControlInbox?.WorkQueueUri ?? string.Empty,
                    ControlInboxErrorQueueUri = _serviceBusOptions.ControlInbox.ErrorQueueUri ?? string.Empty
                };

                foreach (var messageType in _messageTypes)
                {
                    result.MessageTypesHandled.Add(messageType);
                }

                foreach (var association in _associations)
                {
                    result.MessageTypeAssociations.Add(association);
                }

                foreach (var dispatched in _dispatched)
                {
                    result.MessageTypesDispatched.Add(dispatched);
                }

                foreach (var messageTypeMetric in _messageTypeMetrics.Values)
                {
                    result.MessageTypeMetrics.Add(messageTypeMetric);
                }

                _messageTypes.Clear();
                _associations.Clear();
                _dispatched.Clear();
                _messageTypeMetrics.Clear();

                return result;
            }
        }

        public void RegisterAssociation(string messageTypeHandled, string messageTypeDispatched)
        {
            var key = $"{messageTypeHandled}\\{messageTypeDispatched}";

            lock (_lock)
            {
                if (_registeredAssociations.Contains(key))
                {
                    return;
                }

                _associations.Add(new RegisterEndpoint.Association
                {
                    MessageTypeHandled = messageTypeHandled,
                    MessageTypeDispatched = messageTypeDispatched
                });

                _registeredAssociations.Add(key);
            }
        }

        public void RegisterDispatched(string messageType, string recipientInboxWorkQueueUri)
        {
            var key = $"{messageType}\\{recipientInboxWorkQueueUri}";

            lock (_lock)
            {
                if (_registeredAssociations.Contains(key))
                {
                    return;
                }

                _dispatched.Add(new RegisterEndpoint.Dispatched
                {
                    MessageType = messageType,
                    RecipientInboxWorkQueueUri = recipientInboxWorkQueueUri
                });

                _registeredDispatched.Add(key);
            }
        }
    }
}