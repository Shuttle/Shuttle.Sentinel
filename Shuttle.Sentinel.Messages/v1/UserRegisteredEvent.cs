using System;

namespace Shuttle.Sentinel.Messages.v1
{
	public class UserRegisteredEvent
	{
		public Guid Id { get; set; }
		public string EMail { get; set; }
		public string RegisteredBy { get; set; }
		public DateTime DateRegistered { get; set; }
	}
}