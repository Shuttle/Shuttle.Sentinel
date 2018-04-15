using System;

namespace Shuttle.Sentinel.Module
{
    public interface ISentinelConfiguration
    {
        TimeSpan HeartbeatIntervalDuration { get; }
    }
}