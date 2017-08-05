using System;
using System.Configuration;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.Module
{
    public class SentinelSection : ConfigurationSection
    {
        [ConfigurationProperty("inboxWorkQueueUri", IsRequired = true)]
        public string InboxWorkQueueUri => (string)this["inboxWorkQueueUri"];

        [ConfigurationProperty("endpointName", IsRequired = false)]
        public string EndpointName => (string)this["endpointName"];

        [ConfigurationProperty("heartbeatIntervalSeconds", IsRequired = false, DefaultValue = 15)]
        public int HeartbeatIntervalSeconds => (int)this["heartbeatIntervalSeconds"];

        public static ISentinelConfiguration Configuration()
        {
            var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel");

            if (section == null)
            {
                throw new InvalidOperationException("Could not get the Sentinel configuration section.  CIf you have added the section the type and/or namespace may be incorrect.");
            }

            var result = new SentinelConfiguration
            {
                EndpointName = string.IsNullOrEmpty(section.EndpointName) ? GetEndpointName() : section.EndpointName,
                HeartbeatIntervalSeconds = section.HeartbeatIntervalSeconds,
                InboxWorkQueueUri = section.InboxWorkQueueUri
            };

            return result;
        }

        private static string GetEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
            {
                throw new InvalidOperationException("Could not get the entry assembly.");
            }

            return entryAssembly.FullName;
        }
    }
}