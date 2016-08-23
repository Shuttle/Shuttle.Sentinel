using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class RoleHandler :
        IEventHandler<Added>
    {
        private readonly ISystemRoleQuery _query;

        public RoleHandler(ISystemRoleQuery query)
        {
            Guard.AgainstNull(query, "query");

            _query = query;
        }


        public void ProcessEvent(IEventHandlerContext<Added> context)
        {
            _query.Added(context.ProjectionEvent, context.DomainEvent);
        }
    }
}