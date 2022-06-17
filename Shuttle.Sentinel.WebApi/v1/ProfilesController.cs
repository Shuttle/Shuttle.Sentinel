using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Messages.v1;
using Shuttle.Access.RestClient;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Messages.v1;
using Shuttle.Sentinel.WebApi.Models;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IAccessClient _accessClient;
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IProfileQuery _profileQuery;

        public ProfilesController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IEventStore eventStore, IAccessClient accessClient, IProfileQuery profileQuery)
        {
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(eventStore, nameof(eventStore));
            Guard.AgainstNull(accessClient, nameof(accessClient));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));

            _bus = bus;
            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _accessClient = accessClient;
            _profileQuery = profileQuery;
        }

        [HttpGet("identity/{emailAddress}")]
        public IActionResult Get(string emailAddress)
        {
            using (_databaseContextFactory.Create())
            {
                var profile = _profileQuery
                    .Search(new DataAccess.Query.Profile.Specification().WithEMailAddress(emailAddress)
                        .WithEffectiveDate(DateTime.Now)).FirstOrDefault();

                return profile != null
                    ? Ok(new
                    {
                        identityName = Profile.GetIdentityName(profile.SentinelId)
                    })
                    : BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterProfile message)
        {
            Guard.AgainstNull(message, nameof(message));

            try
            {
                message.ApplyInvariants();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _bus.Send(message);

            return Ok();
        }

        [HttpPost("passwordreset")]
        public IActionResult Post([FromBody] PasswordResetModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            try
            {
                model.ApplyInvariants();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _bus.Send(new SendPasswordResetEMail
            {
                EMailAddress = model.EMailAddress
            });

            return Ok();
        }

        [HttpPost("activate/{id}")]
        public IActionResult Post(Guid id)
        {
            using (_databaseContextFactory.Create())
            {
                var profile = new Profile(id);
                var stream = _eventStore.Get(id);

                stream.Apply(profile);

                if (!profile.Activated)
                {
                    _accessClient.Identities.Activate(new ActivateIdentity
                    {
                        Id = id,
                        Name = profile.IdentityName
                    });

                    stream.AddEvent(profile.Activate());

                    _eventStore.Save(stream);
                }
            }

            return Ok(new
            {
                ok = true
            });
        }

        [HttpPost("tokenlogin/{securityToken}")]
        public IActionResult TokenLogin(Guid securityToken)
        {
            DataAccess.Query.Profile queryProfile;

            using (_databaseContextFactory.Create())
            {
                queryProfile = _profileQuery
                    .Search(new DataAccess.Query.Profile.Specification().WithSecurityToken(securityToken))
                    .SingleOrDefault();
            }

            if (queryProfile == null)
            {
                return BadRequest();
            }

            var registerSessionResult = _accessClient.Sessions.Post(new RegisterDelegatedSession
            {
                IdentityName = Profile.GetIdentityName(queryProfile.SentinelId)
            }).Result;

            return registerSessionResult.IsSuccessStatusCode
                ? Ok(new
                {
                    Success = true,
                    registerSessionResult.Content.IdentityName,
                    Token = registerSessionResult.Content.Token.ToString("n"),
                    Permissions = registerSessionResult.Content.Permissions.Select(permission => new
                    {
                        Permission = permission
                    }).ToList()
                })
                : Ok(new
                {
                    Success = false
                });
        }

        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            try
            {
                model.ApplyInvariants();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            DataAccess.Query.Profile queryProfile;

            using (_databaseContextFactory.Create())
            {
                queryProfile = _profileQuery
                    .Search(new DataAccess.Query.Profile.Specification().WithPasswordResetToken(
                        model.PasswordResetToken)).SingleOrDefault();
            }

            if (queryProfile == null)
            {
                return BadRequest();
            }

            _accessClient.Identities.ResetPassword(new ResetPassword
            {
                Name = Profile.GetIdentityName(queryProfile.SentinelId),
                Password = model.Password,
                PasswordResetToken = model.PasswordResetToken
            });

            return Ok();
        }
    }
}