using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class UserHandler :
        IMessageHandler<RegisterUserCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IKeyStore _keyStore;
        private readonly ISystemUserQuery _systemUserQuery;

        public UserHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore,
            ISystemUserQuery systemUserQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(eventStore, "eventStore");
            Guard.AgainstNull(keyStore, "keyStore");
            Guard.AgainstNull(systemUserQuery, "systemUserQuery");

            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _keyStore = keyStore;
            _systemUserQuery = systemUserQuery;
        }

        public void ProcessMessage(IHandlerContext<RegisterUserCommand> context)
        {
            var message = context.Message;

            if (string.IsNullOrEmpty(message.Username))
            {
                return;
            }

            if (string.IsNullOrEmpty(message.RegisteredBy))
            {
                return;
            }

            var id = Guid.NewGuid();

            Registered registered;

            using (_databaseContextFactory.Create())
            {
                var key = User.Key(message.Username);

                if (_keyStore.Contains(key))
                {
                    return;
                }

                var count = _systemUserQuery.Count();

                _keyStore.Add(id, key);

                var user = new User(id);
                var stream = new EventStream(id);

                registered = user.Register(message.Username, message.PasswordHash, message.RegisteredBy);

                if (count == 0)
                {
                    stream.AddEvent(user.AddRole("administrator"));
                }

                stream.AddEvent(registered);

                _eventStore.SaveEventStream(stream);
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}