using Shuttle.Core.Contract;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess.Profile;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel.Projection
{
    public class ProfileHandler :
        IEventHandler<Registered>,
        IEventHandler<PasswordResetRequested>
    {
        private readonly IProfileProjectionQuery _query;

        public ProfileHandler(IProfileProjectionQuery query)
        {
            Guard.AgainstNull(query, nameof(query));

            _query = query;
        }

        public void ProcessEvent(IEventHandlerContext<Registered> context)
        {
            Guard.AgainstNull(context, nameof(context));

            _query.Register(context.PrimitiveEvent, context.Event);
        }

        public void ProcessEvent(IEventHandlerContext<PasswordResetRequested> context)
        {
            Guard.AgainstNull(context, nameof(context));

            _query.PasswordResetToken(context.PrimitiveEvent, context.Event);
        }
    }
}
