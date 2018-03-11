using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Core.Reflection;

namespace Shuttle.Sentinel.DataAccess
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

        public void Save(Guid id, string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Save(id, ipv4Address, inboxWorkQueueUri,
                controlInboxWorkQueueUri));
        }

        public void Add(string machineName, string baseDirectory, string ipv4Address, string inboxWorkQueueUri,
            string controlInboxWorkQueueUri)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Add(machineName, baseDirectory, ipv4Address, inboxWorkQueueUri,
                controlInboxWorkQueueUri));
        }
    }
}