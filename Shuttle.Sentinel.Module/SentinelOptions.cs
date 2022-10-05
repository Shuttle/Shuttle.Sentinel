using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Module
{
    public class SentinelOptions
    {
        public const string SectionName = "Shuttle:Modules:Sentinel";

        public bool Enabled { get; set; } = true;
        public bool TransientInstance { get; set; }
        public int MaximumMessageContentSize { get; set; } = int.MaxValue;
        public TimeSpan HeartbeatIntervalDuration { get; set; } = TimeSpan.FromSeconds(30);
        public List<string> Tags { get; set; }
    }
}