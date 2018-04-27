using System;

namespace Shuttle.Sentinel.WebApi.Configuration
{
	public interface ISentinelConfiguration
	{
		Type SerializerType { get; set; }
	    TimeSpan HeartbeatIntervalDuration { get; set; }
    }
}