using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public SentinelConfiguration()
        {
            MachineName = Environment.MachineName;
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public string InboxWorkQueueUri { get; set; }
        public string EndpointName { get; set; }
        public string MachineName { get; }
        public string BaseDirectory { get; }
    }
}