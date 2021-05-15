using System;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.OAuth;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi.Controllers
{
    [Route("[controller]")]
    public class OAuthController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IWebApiConfiguration _configuration;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IKeyStore _keyStore;
        private readonly IOAuthProviderCollection _oAuthProviderCollection;
        private readonly IProfileQuery _profileQuery;

        public OAuthController(IServiceBus bus, IOAuthProviderCollection oAuthProviderCollection,
            IDatabaseContextFactory databaseContextFactory, IKeyStore keyStore,
            IProfileQuery profileQuery, IWebApiConfiguration configuration)
        {
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(oAuthProviderCollection, nameof(oAuthProviderCollection));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(keyStore, nameof(keyStore));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));
            Guard.AgainstNull(configuration, nameof(configuration));

            _bus = bus;
            _oAuthProviderCollection = oAuthProviderCollection;
            _databaseContextFactory = databaseContextFactory;
            _keyStore = keyStore;
            _profileQuery = profileQuery;
            _configuration = configuration;
        }

        [HttpGet("github")]
        public IActionResult GitHub(string code)
        {
            var emailAddress = GetGitHubEMailAddress(code);

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return Redirect(_configuration.GetUrl("login?message=oauth-email-not-found"));
            }

            var securityToken = Guid.NewGuid();

            using (_databaseContextFactory.Create())
            {
                if (!_profileQuery.Contains(new DataAccess.Query.Profile.Specification().WithEMailAddress(emailAddress)
                    .WithEffectiveDate(DateTime.MaxValue))
                )
                {
                    return Redirect(
                        _configuration.GetUrl($"register?message=email-not-registered&email={emailAddress}"));
                }

                _profileQuery.RegisterSecurityToken(emailAddress, securityToken);
            }

            return Redirect(_configuration.GetUrl($"tokenlogin/{securityToken}"));
        }

        private string GetGitHubEMailAddress(string code)
        {
            return _oAuthProviderCollection.Get("github").GetData(code).email;
        }

        [HttpGet("github-register")]
        public IActionResult GitHubRegister(string code)
        {
            var emailAddress = GetGitHubEMailAddress(code);

            if (string.IsNullOrEmpty(code))
            {
                return Redirect(_configuration.GetUrl("login?message=oauth-email-not-found"));
            }

            using (_databaseContextFactory.Create())
            {
                if (_keyStore.Contains(Profile.Key(emailAddress)))
                {
                    return Redirect(_configuration.GetUrl($"register?message=already-registered&email={emailAddress}"));
                }
            }

            _bus.Send(new RegisterProfileCommand
            {
                EMailAddress = emailAddress
            });

            return Redirect(_configuration.GetUrl($"login?message=register-request-sent&email={emailAddress}"));
        }
    }
}