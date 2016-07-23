using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class UserProjectionHandler :
        IEventHandler<Registered>
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
    }
}