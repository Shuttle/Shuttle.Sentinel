using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RemoveScheduleCommand
    {
        public Guid DataStoreId { get; set; }
        public Guid Id { get; set; }
    }
}