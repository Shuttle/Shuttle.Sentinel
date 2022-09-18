using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelOptions
    {
        public bool Enabled { get; set; }
        public TimeSpan HeartbeatIntervalDuration { get; set; } = TimeSpan.FromSeconds(30);
    }
}