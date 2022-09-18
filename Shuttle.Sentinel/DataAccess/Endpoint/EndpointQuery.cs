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

        public void Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri, string inboxDeferredQueueUri, string inboxErrorQueueUri,
            string outboxWorkQueueUri, string outboxErrorQueueUri, string controlInboxWorkQueueUri,
            string controlInboxErrorQueueUri, string heartbeatIntervalDuration)
        {
            _databaseGateway.Execute(_queryFactory.Save(machineName, baseDirectory, entryAssemblyQualifiedName,
                ipv4Address, inboxWorkQueueUri, inboxDeferredQueueUri, inboxErrorQueueUri, controlInboxWorkQueueUri,
                controlInboxErrorQueueUri, outboxWorkQueueUri, outboxErrorQueueUri, heartbeatIntervalDuration));
        }

        public Guid? FindId(string machineName, string baseDirectory)
        {
            return _databaseGateway.GetScalar<Guid?>(_queryFactory.FindId(
                machineName,
                baseDirectory));
        }

        public void AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count,
            double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
            _databaseGateway.Execute(_queryFactory.AddMessageTypeMetric(metricId, messageType, dateRegistered,
                endpointId,
                count, fastestExecutionDuration, slowestExecutionDuration, totalExecutionDuration));
        }

        public void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched)
        {
            _databaseGateway.Execute(
                _queryFactory.AddMessageTypeAssociation(endpointId, messageTypeHandled, messageTypeDispatched));
        }

        public void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri)
        {
            _databaseGateway.Execute(
                _queryFactory.AddMessageTypeDispatched(endpointId, dispatchedMessageType, recipientInboxWorkQueueUri));
        }

        public void AddMessageTypeHandled(Guid endpointId, string messageType)
        {
            _databaseGateway.Execute(
                _queryFactory.AddMessageTypeHandled(endpointId, messageType));
        }

        public void Remove(Guid id)
        {
            _databaseGateway.Execute(_queryFactory.Remove(id));
        }

        public IEnumerable<Endpoint> All()
        {
            return _queryMapper.MapObjects<Endpoint>(_queryFactory.All());
        }

        public IEnumerable<Endpoint> Search(string match)
        {
            return _queryMapper.MapObjects<Endpoint>(_queryFactory.Search(match));
        }
    }
}