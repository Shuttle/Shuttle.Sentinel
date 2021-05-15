using System;
using System.Linq;
using Shuttle.Access.Api;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.EMail.Messages;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server.Handlers
{
    public class SendPasswordResetEMailHandler : IMessageHandler<SendPasswordResetEMailCommand>
    {
        private readonly IAccessClient _accessClient;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IProfileQuery _profileQuery;
        private readonly ISentinelServerConfiguration _serverConfiguration;

        public SendPasswordResetEMailHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore,
            IProfileQuery profileQuery, ISentinelServerConfiguration serverConfiguration, IAccessClient accessClient)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(eventStore, nameof(eventStore));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));
            Guard.AgainstNull(serverConfiguration, nameof(serverConfiguration));
            Guard.AgainstNull(accessClient, nameof(accessClient));

            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _profileQuery = profileQuery;
            _serverConfiguration = serverConfiguration;
            _accessClient = accessClient;
        }

        public void ProcessMessage(IHandlerContext<SendPasswordResetEMailCommand> context)
        {
            Guard.AgainstNull(context, nameof(context));

            var message = context.Message;

            Profile profile;
            Guid passwordResetToken;

            using (_databaseContextFactory.Create())
            {
                var queryProfile = _profileQuery
                    .Search(new DataAccess.Query.Profile.Specification().WithEMailAddress(message.EMailAddress))
                    .FirstOrDefault();

                if (queryProfile == null)
                {
                    return;
                }

                profile = new Profile(queryProfile.SentinelId);

                var stream = _eventStore.Get(profile.Id);

                stream.Apply(profile);

                passwordResetToken = _accessClient.GetPasswordResetToken(profile.IdentityName);

                stream.AddEvent(profile.RequestPasswordReset(passwordResetToken));

                _eventStore.Save(stream);
            }

            var resetPasswordUrl = $"{_serverConfiguration.ResetPasswordUrl}{passwordResetToken}";
            var sendEMailCommand = new SendEMailCommand
            {
                FromAddress = new SendEMailCommand.Address
                {
                    EMailAddress = _serverConfiguration.NoReplyEMailAddress,
                    DisplayName = _serverConfiguration.NoReplyDisplayName
                },
                Subject = "Sentinel: Password Reset",
                HtmlBody = $@"
<p>Hello,</p>
<p>In order to reset your password please <a href=""{resetPasswordUrl}"">click here</a>.</p>
<p>If the above link does not work please copy the below address and paste it in you browser:</p>
<p>{resetPasswordUrl}</p>
<p>Kind regards,</p>
<p>Shuttle.Sentinel</p>
",
                Body = $@"
Hello,

In order to reset your password please copy the below address and paste it in you browser then press enter:

{resetPasswordUrl}

Kind regards,
Shuttle.Sentinel
"
            };

            sendEMailCommand.ToAddresses.Add(new SendEMailCommand.Address
            {
                EMailAddress = profile.EMailAddress
            });

            context.Send(sendEMailCommand);
        }
    }
}