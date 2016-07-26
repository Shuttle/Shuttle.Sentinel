using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public class SessionQueryFactory : ISessionQueryFactory
	{
		public IQuery Get(Guid token)
		{
			return RawQuery.Create("select Token, Username, DateRegistered from [dbo].[Session] where Token = @Token")
				.AddParameterValue(SessionColumns.Token, token);
		}

		public IQuery GetPermissions(Guid token)
		{
			return RawQuery.Create("select Permission [dbo].[SessionPermission] where Token = @Token")
				.AddParameterValue(SessionColumns.Token, token);
		}

		public IQuery Remove(string username)
		{
			return RawQuery.Create("delete from [dbo].[Session] where Username = @Username")
				.AddParameterValue(SessionColumns.Username, username);
		}

		public IQuery Add(Session session)
		{
			return RawQuery.Create(@"
insert into [dbo].[Session] 
(
	Token, 
	Username, 
	DateRegistered
)
values
(
	@Token, 
	@Username, 
	@DateRegistered
)
")
				.AddParameterValue(SessionColumns.Token, session.Token)
				.AddParameterValue(SessionColumns.Username, session.Username)
				.AddParameterValue(SessionColumns.DateRegistered, session.DateRegistered);
		}

		public IQuery AddPermission(Guid token, string permission)
		{
			return RawQuery.Create(@"
insert into [dbo].[SessionPermission]
(
	Token,
	Permission
)
values
(
	@Token,
	@Permission
)
")
				.AddParameterValue(SessionPermissionColumns.Token, token)
				.AddParameterValue(SessionPermissionColumns.Permission, permission);
		}

		public IQuery Remove(Guid token)
		{
			return RawQuery.Create("delete from [dbo].[Session] where Token = @Token")
				.AddParameterValue(SessionColumns.Token, token);
		}
	}
}