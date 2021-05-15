using System.Configuration;
using Shuttle.Core.Configuration;

namespace Shuttle.Sentinel
{
    public class SentinelServerSection : ConfigurationSection
    {
        [ConfigurationProperty("noReplyEMailAddress", IsRequired = true)]
        public string NoReplyEMailAddress => (string) this["noReplyEMailAddress"];

        [ConfigurationProperty("noReplyDisplayName", IsRequired = true)]
        public string NoReplyDisplayName => (string) this["noReplyDisplayName"];

        [ConfigurationProperty("activationUrl", IsRequired = true)]
        public string ActivationUrl => (string) this["activationUrl"];

        [ConfigurationProperty("resetPasswordUrl", IsRequired = true)]
        public string ResetPasswordUrl => (string) this["resetPasswordUrl"];

        public static ISentinelServerConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<SentinelServerSection>("sentinel", "server");
            var configuration = new SentinelServerConfiguration();

            if (section == null)
            {
                throw new ConfigurationErrorsException(Resources.MissingConfigurationSection);
            }

            configuration.NoReplyEMailAddress = section.NoReplyEMailAddress;
            configuration.NoReplyDisplayName = section.NoReplyDisplayName;
            configuration.ActivationUrl = section.ActivationUrl;
            configuration.ResetPasswordUrl = section.ResetPasswordUrl;

            return configuration;
        }
    }
}