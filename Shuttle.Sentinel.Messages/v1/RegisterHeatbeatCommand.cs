using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterHeatbeatCommand
    {
        public class HeartbeatMetric
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public RegisterHeatbeatCommand()
        {
            Metrics = new List<HeartbeatMetric>();
        }

        public string EndpointName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string IPAddress { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public List<HeartbeatMetric> Metrics { get; set; }
    }
}
