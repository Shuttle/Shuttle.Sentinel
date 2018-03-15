using System;

namespace Shuttle.Sentinel
{
	public class SentinelConfiguration : ISentinelConfiguration
	{
		public string ProviderName { get; set; }
		public string ConnectionString { get; set; }
	    public Type SerializerType { get; set; }
	}
}