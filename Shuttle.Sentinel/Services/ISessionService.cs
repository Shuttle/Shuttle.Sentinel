namespace Shuttle.Sentinel
{
	public interface ISessionService
	{
		RegisterSessionResult Register(string email, string password);
	}
}