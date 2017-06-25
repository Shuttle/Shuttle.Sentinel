using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class PermissionQueryFactory : IPermissionQueryFactory
    {
        public IQuery Available()
        {
            return RawQuery.Create(@"select Permission from AvailablePermission");
        }
    }
}