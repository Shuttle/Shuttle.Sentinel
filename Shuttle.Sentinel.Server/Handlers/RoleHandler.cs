using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RoleHandler :
        IMessageHandler<AddRoleCommand>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEventStore _eventStore;
        private readonly IKeyStore _keyStore;

        public RoleHandler(IDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(eventStore, "eventStore");
            Guard.AgainstNull(keyStore, "keyStore");

            _databaseContextFactory = databaseContextFactory;
            _eventStore = eventStore;
            _keyStore = keyStore;
        }

        public void ProcessMessage(IHandlerContext<AddRoleCommand> context)
        {
            var message = context.Message;

            if (string.IsNullOrEmpty(message.Name))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                var key = Role.Key(message.Name);

                if (_keyStore.Contains(key))
                {
                    return;
                }

                var id = Guid.NewGuid();

                _keyStore.Add(id, key);

                var role = new Role(id);
                var stream = new EventStream(id);

                stream.AddEvent(role.Add(message.Name));

                _eventStore.SaveEventStream(stream);
            }
        }

        public bool IsReusable { get { return true; } }
    }
}