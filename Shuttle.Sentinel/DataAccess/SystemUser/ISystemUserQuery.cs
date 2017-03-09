using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
    public interface ISystemUserQuery
    {
        int Count();
        IEnumerable<DataRow> Search();
        Query.User Get(Guid id);
        IEnumerable<string> Roles(Guid id);

        void Register(PrimitiveEvent primitiveEvent, Registered domainEvent);
        void RoleAdded(PrimitiveEvent primitiveEvent, RoleAdded domainEvent);
        void RoleRemoved(PrimitiveEvent primitiveEvent, RoleRemoved domainEvent);
        int AdministratorCount();
    }
}