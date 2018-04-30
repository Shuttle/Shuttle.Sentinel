using System;

namespace Shuttle.Sentinel.WebApi
{
    public class ScheduleModel
    {
        public Guid DataStoreId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public string CronExpression { get; set; }
        public DateTime? NextNotification { get; set; }
    }
}