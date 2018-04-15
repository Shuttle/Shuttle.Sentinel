using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public SentinelConfiguration()
        {
            HeartbeatIntervalDuration = TimeSpan.FromSeconds(30);
        }

        public TimeSpan HeartbeatIntervalDuration { get; set; }
    }
}