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
        private readonly Dictionary<string, MessageMetric> _messageMetrics = new Dictionary<string, MessageMetric>();
        private readonly PerformanceCounterValue _performanceCounterValue;
        private readonly IServiceBus _serviceBus;
        private DateTime _startDate;

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
            }, configuration.HeartbeatIntervalSeconds);

            _startDate = DateTime.Now;
        }

        public void SendMetrics()
        {
            lock (_lock)
            {
                var command = new RegisterHeartbeatCommand
                {
                    StartDate = _startDate,
                    EndDate = DateTime.Now,
                    EndpointName = _configuration.EndpointName,
                    MachineName = _configuration.MachineName,
                    BaseDirectory = _configuration.BaseDirectory,
                    Metrics = GetHeartbeatMetrics()
                };

                foreach (var metric in _messageMetrics.Values)
                {
                    command.MessageMetrics.Add(metric);
                }

                _serviceBus.Send(command,
                    c => c.WithRecipient(_configuration.InboxWorkQueueUri));

                _messageMetrics.Clear();
                _startDate = DateTime.Now;
            }
        }

        public void AddExecutionDuration(string messageType, double duration)
        {
            lock (_lock)
            {
                if (!_messageMetrics.ContainsKey(messageType))
                {
                    _messageMetrics.Add(messageType, new MessageMetric
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

        private List<SystemMetric> GetHeartbeatMetrics()
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