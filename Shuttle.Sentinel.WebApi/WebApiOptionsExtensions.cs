using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.WebApi
{
    public static class WebApiOptionsExtensions
    {
        public static string GetUrl(this WebApiOptions webApiOptions, string path)
        {
            Guard.AgainstNull(webApiOptions, nameof(webApiOptions));
            Guard.AgainstNullOrEmptyString(path, nameof(path));

            return $"{webApiOptions.SiteUrl}{path}";
        }
    }
}