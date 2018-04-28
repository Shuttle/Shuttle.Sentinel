using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public static readonly TimeSpan DefaultHeartbeatIntervalDuration = TimeSpan.FromSeconds(30);

        public SentinelConfiguration()
        {
            Enabled = true;
            HeartbeatIntervalDuration = DefaultHeartbeatIntervalDuration;
        }

        public bool Enabled { get; set; }
        public TimeSpan HeartbeatIntervalDuration { get; set; }
    }
}