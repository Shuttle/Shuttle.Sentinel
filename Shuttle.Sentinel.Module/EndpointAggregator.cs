using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class EndpointAggregator : IEndpointAggregator
    {
        private readonly List<RegisterMessageTypeAssociations.Association> _associations =
            new List<RegisterMessageTypeAssociations.Association>();

        private readonly List<RegisterMessageTypesDispatched.Dispatched> _dispatched =
            new List<RegisterMessageTypesDispatched.Dispatched>();

        private readonly object _lock = new object();
        private readonly Dictionary<Guid, DateTime> _messageProcessingStartDates = new Dictionary<Guid, DateTime>();

        private readonly Dictionary<Type, RegisterMessageTypeMetrics.MessageTypeMetric> _messageTypeMetrics =
            new Dictionary<Type, RegisterMessageTypeMetrics.MessageTypeMetric>();

        private readonly List<string> _messageTypes = new List<string>();
        private readonly HashSet<string> _registeredAssociations = new HashSet<string>();
        private readonly HashSet<string> _registeredDispatched = new HashSet<string>();
        private readonly HashSet<string> _registeredMessageTypes = new HashSet<string>();
        private readonly List<RegisterEndpointLogEntries.LogEntry> _logEntries = new List<RegisterEndpointLogEntries.LogEntry>();
        private readonly SentinelOptions _sentinelOptions;

        public EndpointAggregator(IOptions<SentinelOptions> sentinelOptions)
        {
            Guard.AgainstNull(sentinelOptions, nameof(sentinelOptions));
            Guard.AgainstNull(sentinelOptions.Value, nameof(sentinelOptions.Value));

            _sentinelOptions = sentinelOptions.Value;
        }

        public void MessageProcessingStart(Guid messageId)
        {
            lock (_lock)
            {
                if (_messageProcessingStartDates.ContainsKey(messageId))
                {
                    _messageProcessingStartDates.Remove(messageId);
                }

                _messageProcessingStartDates.Add(messageId, DateTime.UtcNow);
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

                var duration = (DateTime.UtcNow - startDate).TotalMilliseconds;

                if (!_messageTypeMetrics.TryGetValue(messageType, out var messageTypeMetric))
                {
                    messageTypeMetric = new RegisterMessageTypeMetrics.MessageTypeMetric
                    {
                        MessageType = messageType.FullName,
                        FastestExecutionDuration = double.MaxValue,
                        SlowestExecutionDuration = 0,
                        Count = 0,
                        TotalExecutionDuration = 0
                    };

                    _messageTypeMetrics.Add(messageType, messageTypeMetric);
                }

                messageTypeMetric.Count += 1;

                if (messageTypeMetric.FastestExecutionDuration > duration)
                {
                    messageTypeMetric.FastestExecutionDuration = duration;
                }

                if (messageTypeMetric.SlowestExecutionDuration < duration)
                {
                    messageTypeMetric.SlowestExecutionDuration = duration;
                }

                messageTypeMetric.TotalExecutionDuration += duration;

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

        public IEnumerable<object> GetCommands()
        {
            lock (_lock)
            {
                var result = new List<object>();

                if (_messageTypes.Any())
                {
                    var message = EndpointMessage.Create<RegisterMessageTypesHandled>();

                    result.Add(message);

                    foreach (var messageType in _messageTypes)
                    {
                        if (!message.HasMessageContentSizeAvailable(messageType,
                                _sentinelOptions.MaximumMessageContentSize))
                        {
                            message = EndpointMessage.Create<RegisterMessageTypesHandled>();

                            result.Add(message);
                        }

                        message.MessageTypesHandled.Add(messageType);
                    }
                }

                if (_associations.Any())
                {
                    var message = EndpointMessage.Create<RegisterMessageTypeAssociations>();

                    result.Add(message);

                    foreach (var association in _associations)
                    {
                        if (!message.HasMessageContentSizeAvailable(association,
                                _sentinelOptions.MaximumMessageContentSize))
                        {
                            message = EndpointMessage.Create<RegisterMessageTypeAssociations>();

                            result.Add(message);
                        }

                        message.MessageTypeAssociations.Add(association);
                    }
                }

                if (_dispatched.Any())
                {
                    var message = EndpointMessage.Create<RegisterMessageTypesDispatched>();

                    result.Add(message);

                    foreach (var dispatched in _dispatched)
                    {
                        if (!message.HasMessageContentSizeAvailable(dispatched,
                                _sentinelOptions.MaximumMessageContentSize))
                        {
                            message = EndpointMessage.Create<RegisterMessageTypesDispatched>();

                            result.Add(message);
                        }

                        message.MessageTypesDispatched.Add(dispatched);
                    }
                }

                if (_messageTypeMetrics.Values.Any())
                {
                    var message = EndpointMessage.Create<RegisterMessageTypeMetrics>();

                    result.Add(message);

                    foreach (var messageTypeMetric in _messageTypeMetrics.Values)
                    {
                        if (message.HasMessageContentSizeAvailable(messageTypeMetric,
                                _sentinelOptions.MaximumMessageContentSize))
                        {
                            message = EndpointMessage.Create<RegisterMessageTypeMetrics>();

                            result.Add(message);
                        }
                        
                        message.MessageTypeMetrics.Add(messageTypeMetric);
                    }
                }

                if (_logEntries.Any())
                {
                    var message = EndpointMessage.Create<RegisterEndpointLogEntries>();

                    result.Add(message);

                    foreach (var logEntry in _logEntries)
                    {
                        if (message.HasMessageContentSizeAvailable(logEntry,
                                _sentinelOptions.MaximumMessageContentSize))
                        {
                            message = EndpointMessage.Create<RegisterEndpointLogEntries>();

                            result.Add(message);
                        }

                        message.LogEntries.Add(logEntry);
                    }
                }

                if (!_messageTypes.Any() &&
                    !_associations.Any() &&
                    !_dispatched.Any() &&
                    !_messageTypeMetrics.Any() &&
                    !_logEntries.Any())
                {
                    result.Add(EndpointMessage.Create<RegisterHeartbeat>());
                }

                _messageTypes.Clear();
                _associations.Clear();
                _dispatched.Clear();
                _messageTypeMetrics.Clear();
                _logEntries.Clear();

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

                _associations.Add(new RegisterMessageTypeAssociations.Association
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
                if (_registeredDispatched.Contains(key))
                {
                    return;
                }

                if (!messageType.StartsWith("Shuttle.Sentinel.Messages", StringComparison.InvariantCultureIgnoreCase))
                {
                    _dispatched.Add(new RegisterMessageTypesDispatched.Dispatched
                    {
                        MessageType = messageType,
                        RecipientInboxWorkQueueUri = recipientInboxWorkQueueUri
                    });
                }

                _registeredDispatched.Add(key);
            }
        }

        public void Log(DateTime dateLogged, int logLevel, string category, int eventId, string message, string scope)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            _logEntries.Add(new RegisterEndpointLogEntries.LogEntry
            {
                DateLogged = dateLogged,
                LogLevel = logLevel,
                Category = category,
                EventId = eventId,
                Message = message,
                Scope = scope
            });
        }
    }
}