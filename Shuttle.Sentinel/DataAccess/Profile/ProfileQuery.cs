using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public class ProfileQuery : IProfileQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IQueryMapper _queryMapper;
        private readonly IProfileQueryFactory _queryFactory;

        public ProfileQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            IProfileQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public IEnumerable<Query.Profile> Search(Query.Profile.Specification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return _queryMapper.MapObjects<Query.Profile>(_queryFactory.Search(specification));
        }

        public void RegisterSecurityToken(string emailAddress, Guid securityToken)
        {
            Guard.AgainstNullOrEmptyString(emailAddress, nameof(emailAddress));

            _databaseGateway.Execute(_queryFactory.RegisterSecurityToken(emailAddress, securityToken));
        }

        public void RemoveSecurityToken(Guid securityToken)
        {
            _databaseGateway.Execute(_queryFactory.RemoveSecurityToken(securityToken));
        }

        public bool Contains(Query.Profile.Specification specification)
        {
            return _databaseGateway.GetRows(_queryFactory.Search(specification)).Any();
        }
    }
}