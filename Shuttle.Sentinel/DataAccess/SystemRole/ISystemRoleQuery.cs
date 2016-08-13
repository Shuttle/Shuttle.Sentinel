using System.Collections.Generic;
using System.Data;

namespace Shuttle.Sentinel
{
	public interface ISystemRoleQuery
	{
		IEnumerable<string> Permissions(string roleName);
	    IEnumerable<DataRow> Search();
	}
}