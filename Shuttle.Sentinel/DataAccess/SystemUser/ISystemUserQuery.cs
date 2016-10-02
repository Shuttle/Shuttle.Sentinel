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

        void Register(ProjectionEvent projectionEvent, Registered domainEvent);
        void RoleAdded(ProjectionEvent projectionEvent, RoleAdded domainEvent);
        void RoleRemoved(ProjectionEvent projectionEvent, RoleRemoved domainEvent);
        int AdministratorCount();
    }
}