using System.Collections.Generic;

namespace Shuttle.Sentinel
{
	public interface IAnonymousPermissions
	{
		IEnumerable<string> AnonymousPermissions();
	}
}