using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.InspectionQueue;

namespace Shuttle.Sentinel.Queues
{
    public class DefaultInspectionQueue : IInspectionQueue
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IInspectionQueueQueryFactory _inspectionQueueQueryFactory;

        private readonly ILog _log;

        public DefaultInspectionQueue(IDatabaseContextFactory databaseContextFactory, IDatabaseGateway databaseGateway,
            IInspectionQueueQueryFactory inspectionQueueQueryFactory)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(databaseGateway, "databaseGateway");
            Guard.AgainstNull(inspectionQueueQueryFactory, "inspectionQueueQueryFactory");

            _databaseContextFactory = databaseContextFactory;
            _databaseGateway = databaseGateway;
            _inspectionQueueQueryFactory = inspectionQueueQueryFactory;

            _log = Log.For(this);
        }

        public void Enqueue(TransportMessage transportMessage, Stream stream)
        {
            try
            {
                using (_databaseContextFactory.Create())
                {
                    _databaseGateway.ExecuteUsing(_inspectionQueueQueryFactory.Enqueue(transportMessage, stream));
                }
            }
            catch (Exception ex)
            {
                _log.Error(
                    string.Format(SentinelResources.EnqueueException, transportMessage.MessageId, ex.Message));

                throw;
            }
        }

        public IEnumerable<InspectionMessage> Messages()
        {
            var result = new List<InspectionMessage>();

            using (_databaseContextFactory.Create())
            {
                result.AddRange(
                    _databaseGateway.GetRowsUsing(_inspectionQueueQueryFactory.Messages())
                        .Select(
                            row =>
                                new InspectionMessage(InspectionQueueColumns.MessageId.MapFrom(row),
                                    new MemoryStream(InspectionQueueColumns.MessageBody.MapFrom(row)))));
            }

            return result;
        }

        public void Remove(Guid messageId)
        {
            using (_databaseContextFactory.Create())
            {
                _databaseGateway.ExecuteUsing(_inspectionQueueQueryFactory.Remove(messageId));
            }
        }
    }
}