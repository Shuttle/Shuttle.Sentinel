using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface ISystemUserQuery
    {
        int Count();
        void Register(ProjectionEvent projectionEvent, Registered domainEvent);
        void RoleAdded(ProjectionEvent projectionEvent, RoleAdded domainEvent);
        IEnumerable<DataRow> Search();
        Query.User Get(Guid id);
    }
}