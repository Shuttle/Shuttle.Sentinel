using System;
using Shuttle.Access.Api;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Recall.Sql.Storage;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server.Handlers
{
    public class RegisterProfileHandler : IMessageHandler<RegisterProfileCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IKeyStore _keyStore;
        private readonly IProfileQuery _profileQuery;
        private readonly IAccessClient _accessClient;

        public RegisterProfileHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore, IProfileQuery profileQuery, IAccessClient accessClient)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(eventStore, nameof(eventStore));
            Guard.AgainstNull(keyStore, nameof(keyStore));
            Guard.AgainstNull(profileQuery, nameof(profileQuery));
            Guard.AgainstNull(accessClient, nameof(accessClient));

            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _keyStore = keyStore;
            _profileQuery = profileQuery;
            _accessClient = accessClient;
        }

        public void ProcessMessage(IHandlerContext<RegisterProfileCommand> context)
        {
            Guard.AgainstNull(context, nameof(context));

            var message = context.Message;

            try
            {
                message.ApplyInvariants();
            }
            catch 
            {
                return;
            }

            var id = Guid.NewGuid();

            using (_databaseContextFactory.Create())
            {
                var key = Profile.Key(message.EMailAddress);

                if (_keyStore.Contains(key))
                {
                    return;
                }

                var profile = new Profile(id);
                
                _accessClient.Register(profile.IdentityName, message.Password, "system://sentinel");

                _keyStore.Add(id, key);

                var stream = _eventStore.CreateEventStream(id);
                
                stream.Apply(profile);

                var registered = profile.Register(message.EMailAddress);

                stream.AddEvent(registered);
                
                _eventStore.Save(stream);
            }
        }
    }
}