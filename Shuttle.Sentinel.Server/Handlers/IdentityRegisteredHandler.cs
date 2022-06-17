using System;
using Shuttle.Access.Messages.v1;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.EMail.Messages;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server.Handlers
{
    public class IdentityRegisteredHandler : IMessageHandler<IdentityRegistered>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IProfileQuery _profileQuery;
        private readonly ISentinelServerConfiguration _serverConfiguration;

        public IdentityRegisteredHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IProfileQuery profileQuery, ISentinelServerConfiguration serverConfiguration)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(eventStore, nameof(eventStore));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));
            Guard.AgainstNull(serverConfiguration, nameof(serverConfiguration));
            
            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _profileQuery = profileQuery;
            _serverConfiguration = serverConfiguration;
        }                                       
                                                    
        public void ProcessMessage(IHandlerContext<IdentityRegistered> context)
        {                           
            Guard.AgainstNull(context, nameof(context));

            var message = context.Message;

            if (!(message.System ?? string.Empty).Equals("system://sentinel", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            Guid id;

            try
            {
                id = new Guid(message.Name.Substring(message.Name.LastIndexOf("/", StringComparison.Ordinal) + 1));
            }
            catch 
            {
                throw new ApplicationException(string.Format(Resources.IdExtractionException, message.Name));
            }

            var profile = new Profile(id);

            using (_databaseContextFactory.Create())
            {
                _eventStore.Get(id).Apply(profile);
            }

            var activationUrl = $"{_serverConfiguration.ActivationUrl}{id}";
            var sendEMailCommand = new SendEMailCommand
            {
                
                FromAddress = new SendEMailCommand.Address
                {
                    EMailAddress = _serverConfiguration.NoReplyEMailAddress,
                    DisplayName = _serverConfiguration.NoReplyDisplayName
                },
                Subject = "Sentinel: Please confirm your email address",
                HtmlBody = $@"
<p>Hello,</p>
<p>Thank you for signing up with Sentinel!</p>
<p>In order to activate your profile please <a href=""{activationUrl}"">click here</a>.</p>
<p>If the above link does not work please copy the below address and paste it in you browser:</p>
<p>{activationUrl}</p>
<p>Kind regards,</p>
<p>Shuttle.Sentinel</p>
",
                Body = $@"
Hello,

Thank you for signing up with Sentinel!
                                
In order to activate your profile please copy the below address and paste it in you browser then press enter:

{activationUrl}

Kind regards,
Shuttle.Sentinel
"
            };
            
            sendEMailCommand.ToAddresses.Add(new SendEMailCommand.Address
            {
                EMailAddress = profile.EMailAddress
            });
            
            context.Send(sendEMailCommand);

            context.Publish(new ProfileRegistered
            {
                Id = id,
                EMailAddress = profile.EMailAddress
            });
        }
    }
}