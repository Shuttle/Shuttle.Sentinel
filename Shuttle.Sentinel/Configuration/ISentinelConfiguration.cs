using System;

namespace Shuttle.Sentinel.Configuration
{
	public interface ISentinelConfiguration
	{
		Type AuthenticationServiceType { get; set; }
		Type AuthorizationServiceType { get; set; }
		Type SerializerType { get; set; }
		string ProviderName { get; set; }
		string ConnectionString { get; set; }
	}
}