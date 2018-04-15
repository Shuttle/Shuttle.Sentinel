using System;
using System.ComponentModel;
using System.Configuration;
using Shuttle.Core.Configuration;
using Shuttle.Core.TimeSpanTypeConverters;

namespace Shuttle.Sentinel.Module
{
    public class SentinelSection : ConfigurationSection
    {
        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("heartbeatIntervalDuration", IsRequired = false, DefaultValue = null)]
        public TimeSpan HeartbeatIntervalDuration
        {
            get
            {
                var spans = (TimeSpan[]) this["heartbeatIntervalDuration"];

                return spans.Length > 0 ? spans[0] : TimeSpan.FromSeconds(30);
            }
        }

        public ISentinelConfiguration Configuration()
        {
            var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel");
            var configuration = new SentinelConfiguration();

            if (section != null)
            {
                configuration.HeartbeatIntervalDuration = section.HeartbeatIntervalDuration;
            }

            return configuration;

        }
    }
}