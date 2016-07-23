using System;

namespace Shuttle.Sentinel.Messages.v1
{
	public class UserRegisteredEvent
	{
		public string Username { get; set; }
		public string RegisteredBy { get; set; }
		public DateTime DateRegistered { get; set; }
	}
}