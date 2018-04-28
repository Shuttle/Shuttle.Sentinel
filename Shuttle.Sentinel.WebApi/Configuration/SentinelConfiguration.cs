using System;

namespace Shuttle.Sentinel.WebApi.Configuration
{
	public class SentinelConfiguration : ISentinelConfiguration
	{
	    public static readonly TimeSpan DefaultHeartbeatRecoveryDuration = TimeSpan.FromSeconds(5);

	    public Type SerializerType { get; set; }
	    public TimeSpan HeartbeatRecoveryDuration { get; set; }

	    public SentinelConfiguration()
	    {
            HeartbeatRecoveryDuration = DefaultHeartbeatRecoveryDuration;
	    }
	}
}