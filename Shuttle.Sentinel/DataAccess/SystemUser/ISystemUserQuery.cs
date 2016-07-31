using System.Collections;
using System.Collections.Generic;
using System.Data;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public interface ISystemUserQuery
	{
        int Count();
        void Register(ProjectionEvent projectionEvent, Registered domainEvent);
	    void RoleAdded(ProjectionEvent projectionEvent, RoleAdded domainEvent);
	    IEnumerable<DataRow> Search();
	}
}