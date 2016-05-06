/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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

		public static ISentinelConfiguration Configuration()
		{
			var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel");

			if (section == null)
			{
				throw new ConfigurationErrorsException(SentinelResources.SectionMissing);
			}

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