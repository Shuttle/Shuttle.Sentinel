using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.OAuth;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class OAuthController : Controller
    {
        private readonly IServiceBus _serviceBus;
        private readonly WebApiOptions _webApiOptions;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IKeyStore _keyStore;
        private readonly IOAuthProviderService _oAuthProviderService;
        private readonly IProfileQuery _profileQuery;

        public OAuthController(IOptions<WebApiOptions> webApiOptions, IServiceBus serviceBus, IOAuthProviderService oAuthProviderService,
            IDatabaseContextFactory databaseContextFactory, IKeyStore keyStore,
            IProfileQuery profileQuery)
        {
            Guard.AgainstNull(webApiOptions, nameof(webApiOptions));
            Guard.AgainstNull(webApiOptions.Value, nameof(webApiOptions.Value));
            Guard.AgainstNull(serviceBus, nameof(serviceBus));
            Guard.AgainstNull(oAuthProviderService, nameof(oAuthProviderService));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(keyStore, nameof(keyStore));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));

            _webApiOptions = webApiOptions.Value;
            _serviceBus = serviceBus;
            _oAuthProviderService = oAuthProviderService;
            _databaseContextFactory = databaseContextFactory;
            _keyStore = keyStore;
            _profileQuery = profileQuery;
        }

        [HttpGet("github")]
        public IActionResult GitHub(string code)
        {
            var emailAddress = GetGitHubEMailAddress(code);

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return Redirect(_webApiOptions.GetUrl("login?message=oauth-email-not-found"));
            }

            var securityToken = Guid.NewGuid();

            using (_databaseContextFactory.Create())
            {
                if (!_profileQuery.Contains(new DataAccess.Query.Profile.Specification().WithEMailAddress(emailAddress)
                    .WithEffectiveDate(DateTime.MaxValue))
                )
                {
                    return Redirect(
                        _webApiOptions.GetUrl($"register?message=email-not-registered&email={emailAddress}"));
                }

                _profileQuery.RegisterSecurityToken(emailAddress, securityToken);
            }

            return Redirect(_webApiOptions.GetUrl($"tokenlogin/{securityToken}"));
        }

        private string GetGitHubEMailAddress(string code)
        {
            return _oAuthProviderService.Get("github").GetData(code).email;
        }

        [HttpGet("github-register")]
        public IActionResult GitHubRegister(string code)
        {
            var emailAddress = GetGitHubEMailAddress(code);

            if (string.IsNullOrEmpty(code))
            {
                return Redirect(_webApiOptions.GetUrl("login?message=oauth-email-not-found"));
            }

            using (_databaseContextFactory.Create())
            {
                if (_keyStore.Contains(Profile.Key(emailAddress)))
                {
                    return Redirect(_webApiOptions.GetUrl($"register?message=already-registered&email={emailAddress}"));
                }
            }

            _serviceBus.Send(new RegisterProfile
            {
                EMailAddress = emailAddress
            });

            return Redirect(_webApiOptions.GetUrl($"login?message=register-request-sent&email={emailAddress}"));
        }
    }
}