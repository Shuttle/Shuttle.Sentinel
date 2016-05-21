using System.Collections.Generic;

namespace Shuttle.Sentinel
{
	public interface IAuthorizationService
	{
		IEnumerable<string> Permissions(string email, object authenticationTag);
	}
}