using System;
using System.ComponentModel;
using System.Configuration;
using Shuttle.Core.Configuration;
using Shuttle.Core.TimeSpanTypeConverters;

namespace Shuttle.Sentinel.WebApi.Configuration
{
    public class SentinelSection : ConfigurationSection
    {
        [ConfigurationProperty("serializerType", IsRequired = false,
            DefaultValue = "Shuttle.Core.Serialization.DefaultSerializer, Shuttle.Core.Serialization")]
        public string SerializerType => (string) this["serializerType"];

        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("heartbeatIntervalDuration", IsRequired = false, DefaultValue = null)]
        public TimeSpan HeartbeatIntervalDuration
        {
            get
            {
                var result = this["heartbeatIntervalDuration"];

                if (result is TimeSpan span)
                {
                    return span.Equals(TimeSpan.Zero) ? SentinelConfiguration.DefaultHeartbeatIntervalDuration : span;
                }

                var spans = result as TimeSpan[];

                return spans == null
                    ? SentinelConfiguration.DefaultHeartbeatIntervalDuration
                    : (spans.Length > 0 ? spans[0] : SentinelConfiguration.DefaultHeartbeatIntervalDuration);
            }
        }

        public static ISentinelConfiguration Configuration()
        {
            var section = ConfigurationSectionProvider.Open<SentinelSection>("shuttle", "sentinel") ??
                          new SentinelSection();

            if (section == null)
            {
                throw new InvalidOperationException(Resources.MissingConfigurationSectionException);
            }

            var result = new SentinelConfiguration
            {
                HeartbeatIntervalDuration = section.HeartbeatIntervalDuration
            };

            try
            {
                result.SerializerType = Type.GetType(section.SerializerType, true, true);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.TypeNotFoundException, "SerializerType",
                    section.SerializerType, ex.Message));
            }

            return result;
        }
    }
}