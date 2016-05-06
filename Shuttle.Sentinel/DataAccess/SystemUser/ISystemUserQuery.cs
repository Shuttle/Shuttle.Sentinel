using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public interface ISystemUserQuery
	{
		void Register(ProjectionEvent projectionEvent, Registered domainEvent);
		int Count();
	}
}