using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel
{
    public interface ISystemRoleQueryFactory
    {
        IQuery Permissions(string roleName);
        IQuery Permissions(Guid roleId);
        IQuery Search();
        IQuery Added(Guid id, Added domainEvent);
        IQuery Get(Guid id);
    }
}