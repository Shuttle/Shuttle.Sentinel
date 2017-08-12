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
        private readonly ISentinelConfiguration _configuration;
        private readonly object _lock = new object();
        private readonly List<MessageMetric> _messageMetrics = new List<MessageMetric>();
        private readonly PerformanceCounterValue _performanceCounterValue;
        private readonly IServiceBus _serviceBus;

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
        }

        public void SendMetrics()
        {
            lock (_lock)
            {
                _serviceBus.Send(new RegisterHeartbeatCommand
                    {
                        EndpointName = _configuration.EndpointName,
                        MachineName = _configuration.MachineName,
                        BaseDirectory = _configuration.BaseDirectory,
                        Metrics = GetHeartbeatMetrics(),
                        MessageMetrics = _messageMetrics
                    },
                    c => c.WithRecipient(_configuration.InboxWorkQueueUri));

                _messageMetrics.Clear();
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