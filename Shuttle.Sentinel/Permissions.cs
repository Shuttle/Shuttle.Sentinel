namespace Shuttle.Sentinel
{
    public class Permissions
    {
        public static class Manage
        {
            public const string Messages = "sentinel://messages/manage";
            public const string Monitoring = "sentinel://monitoring/manage";
            public const string Queues = "sentinel://queues/manage";
            public const string Schedules = "sentinel://schedules/manage";
            public const string Subscriptions = "sentinel://subscriptions/manage";
        }
        
        public static class View
        {
            public const string Monitoring = "sentinel://monitoring/view";
            public const string Queues = "sentinel://queues/view";
            public const string Schedules = "sentinel://schedules/view";
            public const string Subscriptions = "sentinel://subscriptions/view";
        }
    }
}