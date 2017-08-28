using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Esb.Module.Throttle;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public class MetricCollector : IMetricCollector
    {
        private static readonly object _lock = new object();

        private readonly ISentinelConfiguration _configuration;
        private static readonly HashSet<string> _messageTypeAssociations = new HashSet<string>();
        private static readonly HashSet<string> _messageTypesDispatched = new HashSet<string>();
        private static readonly HashSet<string> _messageTypesHandled = new HashSet<string>();
        private static readonly List<RegisterMetricsCommand.Association> _messageTypeAssociationsRegistered = new List<RegisterMetricsCommand.Association>();
        private static readonly List<RegisterMetricsCommand.Dispatched> _messageTypesDispatchedRegistered = new List<RegisterMetricsCommand.Dispatched>();
        private static readonly List<string> _messageTypesHandledRegistered = new List<string>();
        private readonly Dictionary<string, MessageTypeMetric> _messageMetrics = new Dictionary<string, MessageTypeMetric>();
        private readonly PerformanceCounterValue _performanceCounterValue;
        private readonly IServiceBus _serviceBus;
        private DateTime _startDate;
        private Guid _metricId;

        public MetricCollector(IServiceBus serviceBus, ISentinelConfiguration configuration)
        {
            Guard.AgainstNull(serviceBus, nameof(serviceBus));
            Guard.AgainstNull(configuration, nameof(configuration));

            _serviceBus = serviceBus;
            _configuration = configuration;
            _performanceCounterValue = new PerformanceCounterValue(new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            }, 1000);

            _startDate = DateTime.Now;
            _metricId = Guid.NewGuid();
        }

        public void SendMetrics()
        {
            lock (_lock)
            {
                var command = new RegisterMetricsCommand
                {
                    MetricId = _metricId,
                    StartDate = _startDate,
                    EndDate = DateTime.Now,
                    EndpointName = _configuration.EndpointName,
                    MachineName = _configuration.MachineName,
                    BaseDirectory = _configuration.BaseDirectory
                };

                foreach (var metric in _messageMetrics.Values)
                {
                    command.MessageTypeMetrics.Add(metric);
                }

                foreach (var messageType in _messageTypesHandledRegistered)
                {
                    command.MessageTypesHandled.Add(messageType);
                }

                foreach (var messageType in _messageTypesDispatchedRegistered)
                {
                    command.MessageTypesDispatched.Add(messageType);
                }

                foreach (var messageTypeAssociation in _messageTypeAssociationsRegistered)
                {
                    command.MessageTypeAssociations.Add(messageTypeAssociation);
                }

                _serviceBus.Send(command,
                    c => c.WithRecipient(_configuration.InboxWorkQueueUri));

                _messageTypesHandledRegistered.Clear();
                _messageTypesDispatchedRegistered.Clear();
                _messageTypeAssociationsRegistered.Clear();
                _messageMetrics.Clear();
                _startDate = DateTime.Now;
                _metricId = Guid.NewGuid();
            }
        }

        public void AddExecutionDuration(string messageType, double duration)
        {
            lock (_lock)
            {
                if (!_messageTypesHandled.Contains(messageType))
                {
                    _messageTypesHandledRegistered.Add(messageType);
                    _messageTypesHandled.Add(messageType);
                }

                if (!_messageMetrics.ContainsKey(messageType))
                {
                    _messageMetrics.Add(messageType, new MessageTypeMetric
                    {
                        MessageType = messageType,
                        SlowestExecutionDuration = 0,
                        FastestExecutionDuration = int.MaxValue
                    });
                }

                var metric = _messageMetrics[messageType];

                metric.Count++;

                if (duration > metric.SlowestExecutionDuration)
                {
                    metric.SlowestExecutionDuration = duration;
                }

                if (duration < metric.FastestExecutionDuration)
                {
                    metric.FastestExecutionDuration = duration;
                }

                metric.TotalExecutionDuration += duration;
            }
        }

        public void AddMessageTypeDispatched(string messageTypeDispatched, string recipientInboxWorkQueueUri)
        {
            var key = $"{messageTypeDispatched}/{recipientInboxWorkQueueUri}";

            lock (_lock)
            {
                if (_messageTypesDispatched.Contains(key))
                {
                    return;
                }

                _messageTypesDispatchedRegistered.Add(new RegisterMetricsCommand.Dispatched
                {
                    MessageType = messageTypeDispatched,
                    RecipientInboxWorkQueueUri = recipientInboxWorkQueueUri
                });
                _messageTypesDispatched.Add(key);
            }
        }

        public void AddMessageTypeAssociation(string messageTypeHandled, string messageTypeDispatched)
        {
            var key = $"{messageTypeHandled}/{messageTypeDispatched}";

            lock (_lock)
            {
                if (_messageTypeAssociations.Contains(key))
                {
                    return;
                }

                _messageTypeAssociationsRegistered.Add(new RegisterMetricsCommand.Association
                {
                    MessageTypeHandled = messageTypeHandled,
                    MessageTypeDispatched = messageTypeDispatched
                });
                _messageTypeAssociations.Add(key);
            }
        }

        private List<SystemMetric> GetSystemMetrics()
        {
            var result = new List<SystemMetric>
            {
                new SystemMetric
                {
                    Name = "% Processor Time",
                    Value = _performanceCounterValue.NextValue().ToString(CultureInfo.InvariantCulture)
                }
            };

            return result;
        }
    }
}