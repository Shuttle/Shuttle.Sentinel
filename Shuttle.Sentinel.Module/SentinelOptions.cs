using System;

namespace Shuttle.Sentinel.Module
{
    public class SentinelOptions
    {
        public bool Enabled { get; set; }
        public bool TransientInstance { get; set; }
        public int MaximumMessageContentSize { get; set; } = int.MaxValue;
        public TimeSpan HeartbeatIntervalDuration { get; set; } = TimeSpan.FromSeconds(30);
    }
}