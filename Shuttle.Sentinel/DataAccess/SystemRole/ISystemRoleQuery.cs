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
	    void Added(ProjectionEvent projectionEvent, Added domainEvent);
	    Query.Role Get(Guid id);
	}
}