using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class UserHandler :
        IEventHandler<Registered>,
        IEventHandler<RoleAdded>,
        IEventHandler<RoleRemoved>
    {
        private readonly ISystemUserQuery _query;

        public UserHandler(ISystemUserQuery query)
        {
            Guard.AgainstNull(query, "query");

            _query = query;
        }

        public void ProcessEvent(IEventHandlerContext<Registered> context)
        {
            _query.Register(context.ProjectionEvent, context.DomainEvent);
        }

        public void ProcessEvent(IEventHandlerContext<RoleAdded> context)
        {
            _query.RoleAdded(context.ProjectionEvent, context.DomainEvent);
        }

        public void ProcessEvent(IEventHandlerContext<RoleRemoved> context)
        {
            _query.RoleRemoved(context.ProjectionEvent, context.DomainEvent);
        }
    }
}