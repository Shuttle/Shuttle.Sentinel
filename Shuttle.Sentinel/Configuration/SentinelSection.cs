using System;
using System.Configuration;
using Shuttle.Core.Configuration;

namespace Shuttle.Sentinel
{
	public class SentinelSection : ConfigurationSection
	{
		[ConfigurationProperty("connectionStringName", IsRequired = false, DefaultValue = "Sentinel")]
		public string ConnectionStringName => (string)this["connectionStringName"];

		[ConfigurationProperty("serializerType", IsRequired = false, DefaultValue = "Shuttle.Core.Serialization.DefaultSerializer, Shuttle.Core.Serialization")]
		public string SerializerType => (string)this["serializerType"];

		public static ISentinelConfiguration Configuration()
		{
			var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel") ?? new SentinelSection();

			var result = new SentinelConfiguration();

			try
			{
				result.SerializerType = Type.GetType(section.SerializerType, true, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(string.Format(Resources.TypeNotFoundException, "SerializerType",
					section.SerializerType, ex.Message));
			}

			var connectionString = ConfigurationManager.ConnectionStrings[section.ConnectionStringName];

			if (connectionString == null)
			{
				throw new ConfigurationErrorsException(string.Format(Core.Data.Resources.ConnectionStringMissing, section.ConnectionStringName));
			}

			result.ProviderName = connectionString.ProviderName;
			result.ConnectionString = connectionString.ConnectionString;

			return result;
		}
	}
}