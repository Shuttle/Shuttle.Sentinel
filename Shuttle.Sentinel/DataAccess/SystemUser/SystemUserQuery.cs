using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
    public class SystemUserQuery : ISystemUserQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ISystemUserQueryFactory _queryFactory;

        public SystemUserQuery(IDatabaseGateway databaseGateway, ISystemUserQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(queryFactory, "queryFactory");

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
        }

        public void Register(ProjectionEvent projectionEvent, Registered domainEvent)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Register(projectionEvent.Id, domainEvent));
        }

        public void RoleAdded(ProjectionEvent projectionEvent, RoleAdded domainEvent)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.RoleAdded(projectionEvent.Id, domainEvent));
        }

        public IEnumerable<DataRow> Search()
        {
            return _databaseGateway.GetRowsUsing(_queryFactory.Search());
        }

        public Query.User Get(Guid id)
        {
            var row = _databaseGateway.GetSingleRowUsing(_queryFactory.Get(id));

            var result = new Query.User
            {
                Id = SystemUserColumns.Id.MapFrom(row),
                DateRegistered = SystemUserColumns.DateRegistered.MapFrom(row),
                RegisteredBy = SystemUserColumns.RegisteredBy.MapFrom(row),
                Username = SystemUserColumns.Username.MapFrom(row)
            };

            foreach (var roleRow in _databaseGateway.GetRowsUsing(_queryFactory.Roles(id)))
            {
                result.Roles.Add(SystemUserRoleColumns.RoleName.MapFrom(roleRow));
            }

            return result;
        }

        public int Count()
        {
            return _databaseGateway.GetScalarUsing<int>(_queryFactory.Count());
        }
    }
}