using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class Session
	{
		private readonly List<string> _permissions = new List<string>();

		public Session(Guid token, string username, DateTime dateRegistered)
		{
			Token = token;
			Username = username;
			DateRegistered = dateRegistered;
		}

		public Guid Token { get; private set; }
		public string Username { get; private set; }
		public DateTime DateRegistered { get; set; }

		public IEnumerable<string> Permissions => new ReadOnlyCollection<string>(_permissions);

		public void AddPermission(string permission)
		{
			Guard.AgainstNullOrEmptyString(permission, "permission");

			if (_permissions.Find(candidate => candidate.Equals(permission, StringComparison.InvariantCultureIgnoreCase)) != null)
			{
				return;
			}

			_permissions.Add(permission);
		}

	    public bool HasPermission(string permission)
	    {
	        return _permissions.Contains(permission);
	    }

	    public void Renew()
	    {
	        Token = Guid.NewGuid();
	    }
	}
}