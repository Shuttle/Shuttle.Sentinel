using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RemoveSchedule
    {
        public Guid DataStoreId { get; set; }
        public Guid Id { get; set; }
    }
}