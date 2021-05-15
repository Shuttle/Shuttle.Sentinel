using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.WebApi
{
    public static class WebApiConfigurationExtensions
    {
        public static string GetUrl(this IWebApiConfiguration configuration, string path)
        {
            Guard.AgainstNull(configuration, nameof(configuration));
            Guard.AgainstNullOrEmptyString(path, nameof(path));

            return $"{configuration.SiteUrl}{path}";
        }
    }
}