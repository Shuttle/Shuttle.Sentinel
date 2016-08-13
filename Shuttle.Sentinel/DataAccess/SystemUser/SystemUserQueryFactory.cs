using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
    public class SystemUserQueryFactory : ISystemUserQueryFactory
    {
        private const string selectClause = @"
select
    Id,
    Username,
    DateRegistered,
    RegisteredBy
from
    dbo.SystemUser
";

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

        public IQuery RoleAdded(Guid id, RoleAdded domainEvent)
        {
            return RawQuery.Create(@"
if not exists(select null from [dbo].[SystemUserRole] where UserId = @UserId and RoleName = @RoleName)
    insert into [dbo].[SystemUserRole]
    (
	    [UserId],
	    [RoleName]
    )
    values
    (
	    @UserId,
	    @RoleName
    )
")
                .AddParameterValue(SystemUserRoleColumns.UserId, id)
                .AddParameterValue(SystemUserRoleColumns.RoleName, domainEvent.Role);
        }

        public IQuery Search()
        {
            return RawQuery.Create(selectClause);
        }

        public IQuery Get(Guid id)
        {
            return RawQuery.Create(string.Concat(selectClause, " where Id = @Id"))
                .AddParameterValue(SystemUserColumns.Id, id);
        }

        public IQuery Roles(Guid id)
        {
            return RawQuery.Create(@"select RoleName from dbo.SystemUserRole where UserId = @UserId")
                .AddParameterValue(SystemUserRoleColumns.UserId, id);
        }
    }
}