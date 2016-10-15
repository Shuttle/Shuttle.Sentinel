using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel
{
	public class SystemRoleQuery : ISystemRoleQuery
	{
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ISystemRoleQueryFactory _queryFactory;
	    private readonly IQueryMapper _queryMapper;

	    public SystemRoleQuery(IDatabaseGateway databaseGateway, ISystemRoleQueryFactory queryFactory, IQueryMapper queryMapper)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(queryFactory, "queryFactory");
            Guard.AgainstNull(queryMapper, "queryMapper");

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
	        _queryMapper = queryMapper;
        }

        public IEnumerable<string> Permissions(string roleName)
        {
            return
                _databaseGateway.GetRowsUsing(_queryFactory.Permissions(roleName))
                    .Select(row => SystemRolePermissionColumns.Permission.MapFrom(row))
                    .ToList();
        }

	    public IEnumerable<DataRow> Search()
	    {
            return _databaseGateway.GetRowsUsing(_queryFactory.Search());
        }

	    public void Added(ProjectionEvent projectionEvent, Added domainEvent)
	    {
            _databaseGateway.ExecuteUsing(_queryFactory.Added(projectionEvent.Id, domainEvent));
        }

	    public void PermissionAdded(ProjectionEvent projectionEvent, PermissionAdded domainEvent)
	    {
            _databaseGateway.ExecuteUsing(_queryFactory.PermissionAdded(projectionEvent.Id, domainEvent));
        }

	    public void PermissionRemoved(ProjectionEvent projectionEvent, PermissionRemoved domainEvent)
	    {
            _databaseGateway.ExecuteUsing(_queryFactory.PermissionRemoved(projectionEvent.Id, domainEvent));
        }

	    public void Removed(ProjectionEvent projectionEvent, Removed domainEvent)
	    {
            _databaseGateway.ExecuteUsing(_queryFactory.Removed(projectionEvent.Id, domainEvent));
        }

	    public Query.Role Get(Guid id)
	    {
	        var result = _queryMapper.MapObject<Query.Role>(_queryFactory.Get(id));

            result.Permissions = new List<string>(_queryMapper.MapValues<string>(_queryFactory.Permissions(id)));

	        return result;
	    }

	    public IEnumerable<string> Permissions(Guid id)
	    {
	        return _queryMapper.MapValues<string>(_queryFactory.Permissions(id));
	    }

	    public IEnumerable<string> AvailablePermissions()
	    {
            return _queryMapper.MapValues<string>(_queryFactory.AvailablePermissions());
        }
	}
}