using Shuttle.Core.Data;

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
    }
}