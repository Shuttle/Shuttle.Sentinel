using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public class SystemUserQueryFactory : ISystemUserQueryFactory
	{
		public IQuery Register(Guid id, Registered domainEvent)
		{
			return RawQuery.Create(@"
insert into [dbo].[SystemUser]
(
	[Id],
	[Username],
	[DateRegistered],
	[RegisteredBy]
)
values
(
	@Id,
	@Username,
	@DateRegistered,
	@RegisteredBy
)
")
				.AddParameterValue(SystemUserColumns.Id, id)
				.AddParameterValue(SystemUserColumns.Username, domainEvent.Username)
				.AddParameterValue(SystemUserColumns.DateRegistered, domainEvent.DateRegistered)
				.AddParameterValue(SystemUserColumns.RegisteredBy, domainEvent.RegisteredBy);
		}

		public IQuery Count()
		{
			return RawQuery.Create("select count(*) as count from dbo.SystemUser");
		}
	}
}