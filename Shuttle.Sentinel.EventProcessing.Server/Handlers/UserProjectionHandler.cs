using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class UserProjectionHandler :
        IEventHandler<Registered>,
        IEventHandler<RoleAdded>
    {
        private readonly ISystemUserQuery _query;

        public UserProjectionHandler(ISystemUserQuery query)
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
    }
}