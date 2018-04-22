using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public SentinelConfiguration()
        {
            Enabled = true;
            HeartbeatIntervalDuration = TimeSpan.FromSeconds(30);
        }

        public bool Enabled { get; set; }
        public TimeSpan HeartbeatIntervalDuration { get; set; }
    }
}