using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Esb.EMail;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterUserHandler : IMessageHandler<RegisterUserCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IKeyStore _keyStore;
        private readonly IHashingService _hashingService;
        private readonly ILog _log;

        public RegisterUserHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore, IHashingService hashingService)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(eventStore, "eventStore");
            Guard.AgainstNull(keyStore, "keyStore");
            Guard.AgainstNull(hashingService, "hashingService");

            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _keyStore = keyStore;
            _hashingService = hashingService;

            _log = Log.For(this);
        }

        public void ProcessMessage(IHandlerContext<RegisterUserCommand> context)
        {
            var message = context.Message;
            var id = Guid.NewGuid();

            Registered registered;

            using (_databaseContextFactory.Create())
            {
                var key = User.Key(message.EMail);

                if (_keyStore.Contains(key))
                {
                    if (!message.EMail.Equals("shuttle", StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.Send(new SendEMailCommand
                        {
                            Body = string.Format("<p>Hello,</p><br/><p>Unfortunately e-mail '{0}' has been assigned before your user could be registered.  Please register again.</p><br/><p>Regards</p>", message.EMail),
                            Subject = "User registration failure",
                            IsBodyHtml = true,
                            To = message.EMail
                        });
                    }

                    return;
                }

                _keyStore.Add(id, key);

                var user = new User(id);
                var stream = new EventStream(id);
                registered = user.Register(message.EMail, message.PasswordHash, message.RegisteredBy);

                stream.AddEvent(registered);

                _eventStore.SaveEventStream(stream);
            }

            context.Publish(new UserRegisteredEvent
            {
                Id = id,
                EMail = message.EMail,
                RegisteredBy = message.RegisteredBy,
                DateRegistered = registered.DateRegistered
            });
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}