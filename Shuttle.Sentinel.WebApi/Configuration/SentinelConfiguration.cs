using System;

namespace Shuttle.Sentinel.WebApi.Configuration
{
	public class SentinelConfiguration : ISentinelConfiguration
	{
	    public static readonly TimeSpan DefaultHeartbeatIntervalDuration = TimeSpan.FromSeconds(30);

	    public Type SerializerType { get; set; }
	    public TimeSpan HeartbeatIntervalDuration { get; set; }

	    public SentinelConfiguration()
	    {
            HeartbeatIntervalDuration = TimeSpan.FromSeconds(30);
	    }
	}
}