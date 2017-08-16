using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterHeartbeatCommand
    {

        public RegisterHeartbeatCommand()
        {
            Metrics = new List<SystemMetric>();
            MessageMetrics = new List<MessageMetric>();
        }

        public string EndpointName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SystemMetric> Metrics { get; set; }
        public List<MessageMetric> MessageMetrics { get; set; }
    }
}
