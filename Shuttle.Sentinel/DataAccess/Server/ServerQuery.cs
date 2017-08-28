using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
    public class ServerQuery : IServerQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IServerQueryFactory _queryFactory;

        public ServerQuery(IDatabaseGateway databaseGateway, IServerQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
        }

        public Guid? FindId(string machineName, string baseDirectory)
        {
            return _databaseGateway.GetScalarUsing<Guid?>(_queryFactory.FindId(machineName, baseDirectory));
        }
    }
}