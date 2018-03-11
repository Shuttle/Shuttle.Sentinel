using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;

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

        public void Register(string endpointName, string machineName, string baseDirectory,
            string entryAssemblyQualifiedName,
            string inboxWorkQueueUri, string controlInboxWorkQueueUri)
        {
            var id = FindId(endpointName, machineName, baseDirectory);

            if (id.HasValue)
            {
                _databaseGateway.ExecuteUsing(_queryFactory.Save(
                    id.Value,
                    entryAssemblyQualifiedName,
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

        public void AddMessageTypeMetric(Guid endpointId, string messageType, int count, double fastestExecutionDuration,
            double slowestExecutionDuration, double totalExecutionDuration)
        {
            throw new NotImplementedException();
        }

        public void AddMessageTypeAssociation(Guid endpointId, string messageTypeHandled, string messageTypeDispatched)
        {
            throw new NotImplementedException();
        }

        public void AddMessageTypeDispatched(Guid endpointId, string dispatchedMessageType, string recipientInboxWorkQueueUri)
        {
            throw new NotImplementedException();
        }

        public void AddMessageTypeHandled(Guid endpointId, string messageType)
        {
            throw new NotImplementedException();
        }
    }
}