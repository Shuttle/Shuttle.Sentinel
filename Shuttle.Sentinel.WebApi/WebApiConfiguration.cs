using Shuttle.Core.Configuration;

namespace Shuttle.Sentinel.WebApi
{
    public class WebApiConfiguration : IWebApiConfiguration
    {
        public WebApiConfiguration()
        {
            SiteUrl = ConfigurationItem<string>.ReadSetting("siteUrl").GetValue();

            if (!SiteUrl.EndsWith("/"))
            {
                SiteUrl = $"{SiteUrl}/";
            }
        }

        public string SiteUrl { get; }
    }
}