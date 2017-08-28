using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
    public class EndpointQuery : IEndpointQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IEndpointQueryFactory _queryFactory;

        public EndpointQuery(IDatabaseGateway databaseGateway, IEndpointQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
        }

        public void Register(string endpointName, string machineName, string baseDirectory, string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string controlInboxWorkQueueUri)
        {
            var id = FindId(endpointName, machineName, baseDirectory);

            if (id.HasValue)
            {
                _databaseGateway.ExecuteUsing(_queryFactory.Save(
                    id.Value,
                    entryAssemblyQualifiedName,
                    ipv4Address,
                    inboxWorkQueueUri,
                    controlInboxWorkQueueUri));
            }
            else
            {
                _databaseGateway.ExecuteUsing(_queryFactory.Add(
                    endpointName,
                    machineName,
                    baseDirectory,
                    entryAssemblyQualifiedName,
                    ipv4Address,
                    inboxWorkQueueUri,
                    controlInboxWorkQueueUri));
            }
        }

        public Guid? FindId(string endpointName, string machineName, string baseDirectory)
        {
            return _databaseGateway.GetScalarUsing<Guid?>(_queryFactory.FindId(
                endpointName,
                machineName,
                baseDirectory));
        }
    }
}