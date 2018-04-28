using System;

namespace Shuttle.Sentinel.WebApi.Configuration
{
	public interface ISentinelConfiguration
	{
		Type SerializerType { get; set; }
	    TimeSpan HeartbeatRecoveryDuration { get; set; }
    }
}