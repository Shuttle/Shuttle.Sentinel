using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class EndpointQuery : IEndpointQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IEndpointQueryFactory _queryFactory;
        private readonly IQueryMapper _queryMapper;

        public EndpointQuery(IDatabaseGateway databaseGateway, IQueryMapper queryMapper,
            IEndpointQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryMapper, nameof(queryMapper));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryMapper = queryMapper;
            _queryFactory = queryFactory;
        }

        public void Started(string machineName, string baseDirectory, string environmentName,
            string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string outboxWorkQueueUri, string outboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, bool transientInstance, string heartbeatIntervalDuration,
            DateTime dateStarted)
        {
            _databaseGateway.Execute(_queryFactory.Started(machineName, baseDirectory, environmentName, entryAssemblyQualifiedName,
                ipv4Address, inboxWorkQueueUri, inboxDeferredQueueUri, inboxErrorQueueUri, controlInboxWorkQueueUri,
                controlInboxErrorQueueUri, outboxWorkQueueUri, outboxErrorQueueUri, transientInstance, heartbeatIntervalDuration, dateStarted));
        }

        public Guid? FindId(string machineName, string baseDirectory)
        {
            return _databaseGateway.GetScalar<Guid?>(_queryFactory.FindId(
                machineName,
                baseDirectory));
        }

        public void Remove(Guid endpointId)
        {
            _databaseGateway.Execute(_queryFactory.Remove(endpointId));
        }

        public IEnumerable<Endpoint> Search(string match)
        {
            return _queryMapper.MapObjects<Endpoint>(_queryFactory.Search(match));
        }

        public void RegisterHeartbeat(Guid endpointId)
        {
            _databaseGateway.Execute(_queryFactory.RegisterHeartbeat(endpointId));
        }

        public void Stopped(Guid endpointId, DateTime dateStopped)
        {
            _databaseGateway.Execute(_queryFactory.Stopped(endpointId, dateStopped));
        }

        public void RegisterSystemMetric(Guid endpointId, DateTime dateRegistered, string name, decimal value)
        {
            _databaseGateway.Execute(_queryFactory.RegisterSystemMetric(endpointId, dateRegistered, name, value));
        }
    }
}