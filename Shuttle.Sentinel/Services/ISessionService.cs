using System;

namespace Shuttle.Sentinel
{
	public interface ISessionService
	{
		RegisterSessionResult Register(string username, string password, Guid token);
	}
}