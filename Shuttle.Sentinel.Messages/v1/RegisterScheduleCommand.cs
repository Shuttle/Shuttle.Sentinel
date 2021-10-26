using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterScheduleCommand
    {
        public Guid DataStoreId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public DateTime? NextNotification { get; set; }
    }
}