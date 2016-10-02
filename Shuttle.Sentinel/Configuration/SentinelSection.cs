using System;
using System.Configuration;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class SentinelSection : ConfigurationSection
	{
		[ConfigurationProperty("authenticationServiceType", IsRequired = false,
			DefaultValue = "Shuttle.Sentinel.DefaultAuthenticationService, Shuttle.Sentinel")]
		public string AuthenticationServiceType => (string)this["authenticationServiceType"];

		[ConfigurationProperty("authorizationServiceType", IsRequired = false,
			DefaultValue = "Shuttle.Sentinel.DefaultAuthorizationService, Shuttle.Sentinel")]
		public string AuthorizationServiceType => (string)this["authorizationServiceType"];

		[ConfigurationProperty("connectionStringName", IsRequired = false, DefaultValue = "Sentinel")]
		public string ConnectionStringName => (string)this["connectionStringName"];

		[ConfigurationProperty("serializerType", IsRequired = false, DefaultValue = "Shuttle.Core.DefaultSerializer, Shuttle.Core")]
		public string SerializerType => (string)this["serializerType"];

		public static ISentinelConfiguration Configuration()
		{
			var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel") ?? new SentinelSection();

			var result = new SentinelConfiguration();

			try
			{
				result.AuthenticationServiceType = Type.GetType(section.AuthenticationServiceType, true, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(string.Format(SentinelResources.TypeNotFoundException, "AuthenticationServiceType",
					section.AuthenticationServiceType, ex.Message));
			}

			try
			{
				result.AuthorizationServiceType = Type.GetType(section.AuthorizationServiceType, true, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(string.Format(SentinelResources.TypeNotFoundException, "AuthorizationServiceType",
					section.AuthorizationServiceType, ex.Message));
			}

			try
			{
				result.SerializerType = Type.GetType(section.SerializerType, true, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(string.Format(SentinelResources.TypeNotFoundException, "SerializerType",
					section.SerializerType, ex.Message));
			}

			var connectionString = ConfigurationManager.ConnectionStrings[section.ConnectionStringName];

			if (connectionString == null)
			{
				throw new ConfigurationErrorsException(string.Format(DataResources.ConnectionStringMissing, section.ConnectionStringName));
			}

			result.ProviderName = connectionString.ProviderName;
			result.ConnectionString = connectionString.ConnectionString;

			return result;
		}
	}
}