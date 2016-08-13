using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface ISystemRoleQueryFactory
    {
        IQuery Permissions(string roleName);
        IQuery Search();
    }
}