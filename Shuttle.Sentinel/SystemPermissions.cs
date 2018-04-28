namespace Shuttle.Sentinel
{
    public class SystemPermissions
    {
        public static class Manage
        {
            public const string DataStores = "sentinel://data-stores/manage";
            public const string Messages = "sentinel://messages/manage";
            public const string Monitoring = "sentinel://monitoring/manage";
            public const string Queues = "sentinel://queues/manage";
            public const string Schedules = "sentinel://schedules/manage";
            public const string Subscriptions = "sentinel://subscriptions/manage";
        }
    }
}