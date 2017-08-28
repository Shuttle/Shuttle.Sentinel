using System;
using System.Configuration;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.Module
{
    public class SentinelSection : ConfigurationSection
    {
        [ConfigurationProperty("inboxWorkQueueUri", IsRequired = true)]
        public string InboxWorkQueueUri => (string) this["inboxWorkQueueUri"];

        [ConfigurationProperty("endpointName", IsRequired = false)]
        public string EndpointName => (string) this["endpointName"];

        [ConfigurationProperty("notificationIntervalSeconds", IsRequired = false, DefaultValue = 15)]
        public int NotificationIntervalSeconds => (int) this["notificationIntervalSeconds"];

        public static ISentinelConfiguration Configuration()
        {
            var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel");

            if (section == null)
            {
                return null;
            }

            var result = new SentinelConfiguration
            {
                EndpointName = string.IsNullOrEmpty(section.EndpointName) ? GetEndpointName() : section.EndpointName,
                NotificationIntervalSeconds = section.NotificationIntervalSeconds,
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