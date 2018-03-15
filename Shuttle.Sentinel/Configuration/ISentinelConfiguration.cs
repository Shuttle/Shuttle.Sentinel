using System;

namespace Shuttle.Sentinel
{
	public interface ISentinelConfiguration
	{
		Type SerializerType { get; set; }
		string ProviderName { get; set; }
		string ConnectionString { get; set; }
	}
}