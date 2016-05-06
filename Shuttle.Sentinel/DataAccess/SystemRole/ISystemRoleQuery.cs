using System.Collections.Generic;

namespace Shuttle.Sentinel
{
	public interface ISystemRoleQuery
	{
		IEnumerable<string> Permissions(string roleName);
	}
}