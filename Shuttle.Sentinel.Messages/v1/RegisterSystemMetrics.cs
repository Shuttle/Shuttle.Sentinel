using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterSystemMetrics : EndpointMessage
    {
        public List<SystemMetric> SystemMetrics { get; set; } = new List<SystemMetric>();

        public class SystemMetric
        {
            public string Name { get; set; }
            public decimal Value { get; set; }
            public DateTime DateRegistered { get; set; }
        }
    }
}