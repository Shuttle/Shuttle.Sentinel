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

	    public int Count()
		{
			return _databaseGateway.GetScalarUsing<int>(_queryFactory.Count());
		}
	}
}