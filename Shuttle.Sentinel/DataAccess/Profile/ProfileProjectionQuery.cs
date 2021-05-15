using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Recall;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public class ProfileProjectionQuery : IProfileProjectionQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IProfileQueryFactory _queryFactory;

        public ProfileProjectionQuery(IDatabaseGateway databaseGateway, IProfileQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
        }

        public void Register(PrimitiveEvent primitiveEvent, Registered domainEvent)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Register(primitiveEvent.Id, domainEvent));
        }

        public void PasswordResetToken(PrimitiveEvent primitiveEvent, PasswordResetRequested domainEvent)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.PasswordResetToken(primitiveEvent.Id, domainEvent));
        }
    }
}