using System;

namespace Shuttle.Sentinel
{
	public class SentinelConfiguration : ISentinelConfiguration
	{
		public Type AuthenticationServiceType { get; set; }
		public Type AuthorizationServiceType { get; set; }
		public string ProviderName { get; set; }
		public string ConnectionString { get; set; }
	    public Type SerializerType { get; set; }
	}
}