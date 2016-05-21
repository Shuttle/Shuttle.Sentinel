namespace Shuttle.Sentinel
{
	public interface IAuthenticationService
	{
		AuthenticationResult Authenticate(string username, string password);
	}
}