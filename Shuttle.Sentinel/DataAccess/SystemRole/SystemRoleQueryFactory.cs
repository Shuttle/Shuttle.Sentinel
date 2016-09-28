using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel
{
    public class SystemRoleQueryFactory : ISystemRoleQueryFactory
    {
        public IQuery Permissions(string roleName)
        {
            return RawQuery.Create(@"
select 
    Permission
from
    SystemRolePermission rp
inner join
    SystemRole r on (rp.RoleId = r.Id)
where
    r.RoleName = @RoleName")
                .AddParameterValue(SystemRoleColumns.RoleName, roleName);
        }

        public IQuery Permissions(Guid roleId)
        {
            return RawQuery.Create(@"
select 
    Permission
from
    SystemRolePermission
where
    RoleId = @RoleId
")
                .AddParameterValue(SystemRolePermissionColumns.RoleId, roleId);
        }

        public IQuery Search()
        {
            return RawQuery.Create(@"
select
    Id,
    RoleName
from
    SystemRole
");
        }

        public IQuery Added(Guid id, Added domainEvent)
        {
            return RawQuery.Create(@"
insert into [dbo].[SystemRole]
(
	[Id],
	[RoleName]
)
values
(
	@Id,
	@RoleName
)
")
                .AddParameterValue(SystemRoleColumns.Id, id)
                .AddParameterValue(SystemRoleColumns.RoleName, domainEvent.Name);
        }

        public IQuery Get(Guid id)
        {
            return RawQuery.Create(@"
select
    RoleName as Name
from
    SystemRole
where
    Id = @Id
")
                .AddParameterValue(SystemRoleColumns.Id, id);
        }

        public IQuery AvailablePermissions()
        {
            return RawQuery.Create(@"select Permission from AvailablePermission");
        }
    }
}