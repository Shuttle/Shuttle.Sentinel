using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;

namespace Shuttle.Sentinel.DataAccess
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

        public void Save(string machineName, string baseDirectory, string entryAssemblyQualifiedName,
            string ipv4Address, string inboxWorkQueueUri,
            string inboxDeferredQueueUri, string inboxErrorQueueUri, string outboxWorkQueueUri,
            string outboxErrorQueueUri,
            string controlInboxWorkQueueUri, string controlInboxErrorQueueUri)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Save(machineName, baseDirectory, entryAssemblyQualifiedName,
                ipv4Address, inboxWorkQueueUri, inboxDeferredQueueUri, inboxErrorQueueUri, controlInboxWorkQueueUri,
                controlInboxErrorQueueUri, outboxWorkQueueUri, outboxErrorQueueUri));
        }

        public Guid? FindId(string machineName, string baseDirectory)
        {
            return _databaseGateway.GetScalarUsing<Guid?>(_queryFactory.FindId(
                machineName,
                baseDirectory));
        }

        public void AddMessageTypeMetric(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId,
            int count,
            double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.AddMessageTypeMetric(metricId, messageType, dateRegistered, endpointId,
                count, fastestExecutionDuration, slowestExecutionDuration, totalExecutionDuration));
        }

        public void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched)
        {
            _databaseGateway.ExecuteUsing(
                _queryFactory.AddMessageTypeAssociation(endpointId, messageTypeHandled, messageTypeDispatched));
        }

        public void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType,
            string recipientInboxWorkQueueUri)
        {
            _databaseGateway.ExecuteUsing(
                _queryFactory.AddMessageTypeDispatched(endpointId, dispatchedMessageType, recipientInboxWorkQueueUri));
        }

        public void AddMessageTypeHandled(Guid endpointId, string messageType)
        {
            _databaseGateway.ExecuteUsing(
                _queryFactory.AddMessageTypeHandled(endpointId, messageType));
        }
    }
}