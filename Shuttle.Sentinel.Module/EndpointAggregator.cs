using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Shuttle.Core.Contract;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class EndpointAggregator : IEndpointAggregator
    {
        private readonly List<RegisterEndpointCommand.Association> _associations =
            new List<RegisterEndpointCommand.Association>();

        private readonly List<RegisterEndpointCommand.Dispatched> _dispatched =
            new List<RegisterEndpointCommand.Dispatched>();

        private readonly string _ipv4Address;
        private readonly object _lock = new object();
        private readonly Dictionary<Guid, DateTime> _messageProcessingStartDates = new Dictionary<Guid, DateTime>();

        private readonly Dictionary<Type, RegisterEndpointCommand.MessageTypeMetric> _messageTypeMetrics =
            new Dictionary<Type, RegisterEndpointCommand.MessageTypeMetric>();

        private readonly List<string> _messageTypes = new List<string>();
        private readonly HashSet<string> _registeredAssociations = new HashSet<string>();
        private readonly HashSet<string> _registeredDispatched = new HashSet<string>();
        private readonly HashSet<string> _registeredMessageTypes = new HashSet<string>();
        private readonly IServiceBusConfiguration _serviceBusConfiguration;
        private readonly string _entryAssemblyQualifiedName;

        public EndpointAggregator(IServiceBusConfiguration serviceBusConfiguration)
        {
            Guard.AgainstNull(serviceBusConfiguration, nameof(serviceBusConfiguration));

            _serviceBusConfiguration = serviceBusConfiguration;

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
                    messageTypeMetric = new RegisterEndpointCommand.MessageTypeMetric
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

        public RegisterEndpointCommand GetRegisterEndpointCommand()
        {
            lock (_lock)
            {
                var result = new RegisterEndpointCommand
                {
                    MachineName = Environment.MachineName,
                    IPv4Address = _ipv4Address,
                    BaseDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    EntryAssemblyQualifiedName = _entryAssemblyQualifiedName,
                    InboxWorkQueueUri = _serviceBusConfiguration.Inbox.WorkQueueUri,
                    InboxDeferredQueueUri = _serviceBusConfiguration.Inbox.HasDeferredQueue
                        ? _serviceBusConfiguration.Inbox.DeferredQueueUri
                        : string.Empty,
                    InboxErrorQueueUri = _serviceBusConfiguration.Inbox.ErrorQueueUri,
                    OutboxWorkQueueUri = _serviceBusConfiguration.HasOutbox
                        ? _serviceBusConfiguration.Outbox.WorkQueueUri
                        : string.Empty,
                    OutboxErrorQueueUri = _serviceBusConfiguration.HasOutbox
                        ? _serviceBusConfiguration.Outbox.ErrorQueueUri
                        : string.Empty,
                    ControlInboxWorkQueueUri = _serviceBusConfiguration.HasControlInbox
                        ? _serviceBusConfiguration.ControlInbox.WorkQueueUri
                        : string.Empty,
                    ControlInboxErrorQueueUri = _serviceBusConfiguration.HasControlInbox
                        ? _serviceBusConfiguration.ControlInbox.ErrorQueueUri
                        : string.Empty
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

                _associations.Add(new RegisterEndpointCommand.Association
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

                _dispatched.Add(new RegisterEndpointCommand.Dispatched
                {
                    MessageType = messageType,
                    RecipientInboxWorkQueueUri = recipientInboxWorkQueueUri
                });

                _registeredDispatched.Add(key);
            }
        }
    }
}