using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class SystemRoleQuery : ISystemRoleQuery
	{
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ISystemRoleQueryFactory _queryFactory;

        public SystemRoleQuery(IDatabaseGateway databaseGateway, ISystemRoleQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(queryFactory, "queryFactory");

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
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
    }
}