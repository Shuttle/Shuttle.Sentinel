using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Recall.SqlServer;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class UserProjectionHandler :
        IEventHandler<Registered>
    {
        private readonly IProjectionConfiguration _configuration;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ISystemUserQuery _query;

        public UserProjectionHandler(IProjectionConfiguration configuration, IDatabaseContextFactory databaseContextFactory,
            ISystemUserQuery query)
        {
            Guard.AgainstNull(configuration, "configuration");
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(query, "query");

            _configuration = configuration;
            _databaseContextFactory = databaseContextFactory;
            _query = query;
        }

        public void ProcessEvent(IEventHandlerContext<Registered> context)
        {
            _query.Register(context.ProjectionEvent, context.DomainEvent);
        }
    }
}