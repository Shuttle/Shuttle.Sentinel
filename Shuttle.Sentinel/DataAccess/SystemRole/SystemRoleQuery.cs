using System.Collections.Generic;

namespace Shuttle.Sentinel
{
	public class SystemRoleQuery : ISystemRoleQuery
	{
		public IEnumerable<string> Permissions(string roleName)
		{
			return new List<string>();
		}
	}
}