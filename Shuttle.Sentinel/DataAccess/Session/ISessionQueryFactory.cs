using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public interface ISessionQueryFactory
	{
		IQuery Get(Guid token);
		IQuery GetPermissions(Guid token);
		IQuery Remove(string email);
		IQuery Add(Session session);
		IQuery AddPermission(Guid token, string permission);
		IQuery Remove(Guid token);
	}
}