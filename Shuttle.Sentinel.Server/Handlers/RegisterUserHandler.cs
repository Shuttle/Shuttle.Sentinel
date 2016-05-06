using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.EMail;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
	public class RegisterUserHandler : IMessageHandler<RegisterUserCommand>
	{
		private readonly IConfiguredDatabaseContextFactory _databaseContextFactory;
		private readonly IEventStore _eventStore;
		private readonly IKeyStore _keyStore;
		private readonly IHashingService _hashingService;
		private readonly ILog _log;

		public RegisterUserHandler(IConfiguredDatabaseContextFactory databaseContextFactory, IEventStore eventStore, IKeyStore keyStore, IHashingService hashingService)
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

			if (_keyStore.Contains(User.Key(message.EMail)))
			{
				context.Send(new SendEMailCommand
				{
					Body = string.Format("<p>Hello,</p><br/><p>Unfortunately e-mail '{0}' has been assigned before your user could be registered.  Please register again.</p><br/><p>Regards</p>", message.EMail),
					Subject = "User registration failure",
					IsBodyHtml = true,
					To = message.EMail
				});

				return;
			}

			var id = Guid.NewGuid();
			var user = new User(id);
			var stream = new EventStream(id);
			var registered = user.Register(message.EMail, message.PasswordHash, message.RegisteredBy);

			stream.AddEvent(registered);

			using (_databaseContextFactory.Create())
			{
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