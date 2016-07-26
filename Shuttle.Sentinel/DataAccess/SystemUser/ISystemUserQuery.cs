using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public interface ISystemUserQuery
	{
        int Count();
        void Register(ProjectionEvent projectionEvent, Registered domainEvent);
	    void RoleAdded(ProjectionEvent projectionEvent, RoleAdded domainEvent);
	}
}