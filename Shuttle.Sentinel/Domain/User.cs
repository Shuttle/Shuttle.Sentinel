using System;
using System.Linq;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public class User
	{
		private readonly Guid _id;
		private DateTime? _dateActivated;
		private byte[] _passwordHash;
		private string _registeredBy;
		private string _username;
		private DateTime _dateRegistered;

		public User(Guid id)
		{
			_id = id;
		}

		public Registered Register(string username, byte[] passwordHash, string registeredBy)
		{
			return On(new Registered
			{
				Username = username,
				PasswordHash = passwordHash,
				RegisteredBy = registeredBy,
				DateRegistered = DateTime.Now
			});
		}

		public Registered On(Registered registered)
		{
			Guard.AgainstNull(registered, "registered");

			_username = registered.Username;
			_passwordHash = registered.PasswordHash;
			_registeredBy = registered.RegisteredBy;
			_dateRegistered = registered.DateRegistered;

			return registered;
		}

		public static string Key(string username)
		{
			return string.Format("[user]:username={0};", username);
		}

		public bool PasswordMatches(byte[] hash)
		{
			Guard.AgainstNull(hash, "hash");

			return _passwordHash.SequenceEqual(hash);
		}

		public Activated Activate()
		{
			return On(new Activated());
		}

		public Activated On(Activated activated)
		{
			Guard.AgainstNull(activated, "activated");

			_dateActivated = activated.DateActivated;

			return activated;
		}

		public bool Active
		{
			get { return _dateActivated.HasValue; }
		}
	}
}