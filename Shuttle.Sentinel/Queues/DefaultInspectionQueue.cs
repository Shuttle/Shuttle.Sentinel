using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.Queues
{
    public class DefaultInspectionQueue : IInspectionQueue
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IInspectionQueueQueryFactory _inspectionQueueQueryFactory;

        public DefaultInspectionQueue(IDatabaseContextFactory databaseContextFactory, IDatabaseGateway databaseGateway,
            IInspectionQueueQueryFactory inspectionQueueQueryFactory)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(inspectionQueueQueryFactory, nameof(inspectionQueueQueryFactory));

            _databaseContextFactory = databaseContextFactory;
            _databaseGateway = databaseGateway;
            _inspectionQueueQueryFactory = inspectionQueueQueryFactory;
        }

        public void Enqueue(string sourceQueueUri, TransportMessage transportMessage, Stream stream)
        {
            using (_databaseContextFactory.Create())
            {
                _databaseGateway.Execute(_inspectionQueueQueryFactory.Enqueue(sourceQueueUri, transportMessage,
                    stream));
            }
        }

        public IEnumerable<InspectionMessage> Messages()
        {
            var result = new List<InspectionMessage>();

            using (_databaseContextFactory.Create())
            {
                result.AddRange(
                    _databaseGateway.GetRows(_inspectionQueueQueryFactory.Messages())
                        .Select(
                            row =>
                                new InspectionMessage(
                                    InspectionQueueColumns.SourceQueueUri.MapFrom(row),
                                    InspectionQueueColumns.MessageId.MapFrom(row),
                                    new MemoryStream(InspectionQueueColumns.MessageBody.MapFrom(row)))));
            }

            return result;
        }

        public void Remove(Guid messageId)
        {
            using (_databaseContextFactory.Create())
            {
                _databaseGateway.Execute(_inspectionQueueQueryFactory.Remove(messageId));
            }
        }

        public InspectionMessage Get(Guid messageId)
        {
            using (_databaseContextFactory.Create())
            {
                var row = _databaseGateway.GetRow(_inspectionQueueQueryFactory.Get(messageId));

                return new InspectionMessage(
                    InspectionQueueColumns.SourceQueueUri.MapFrom(row),
                    InspectionQueueColumns.MessageId.MapFrom(row),
                    new MemoryStream(InspectionQueueColumns.MessageBody.MapFrom(row)));
            }
        }
    }
}