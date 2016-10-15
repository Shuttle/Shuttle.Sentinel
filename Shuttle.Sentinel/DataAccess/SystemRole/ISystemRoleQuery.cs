using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel
{
	public interface ISystemRoleQuery
	{
		IEnumerable<string> Permissions(string roleName);
	    IEnumerable<DataRow> Search();
	    Query.Role Get(Guid id);
	    IEnumerable<string> Permissions(Guid id);
	    IEnumerable<string> AvailablePermissions();

        void Added(ProjectionEvent projectionEvent, Added domainEvent);
        void PermissionAdded(ProjectionEvent projectionEvent, PermissionAdded domainEvent);
	    void PermissionRemoved(ProjectionEvent projectionEvent, PermissionRemoved domainEvent);
	    void Removed(ProjectionEvent projectionEvent, Removed domainEvent);
	}
}