using System;

namespace Shuttle.Sentinel.Module
{
    public interface ISentinelConfiguration
    {
        bool Enabled { get; }
        TimeSpan HeartbeatIntervalDuration { get; }
    }
}