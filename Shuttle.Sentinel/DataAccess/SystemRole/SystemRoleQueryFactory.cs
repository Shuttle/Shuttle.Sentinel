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
    }
}