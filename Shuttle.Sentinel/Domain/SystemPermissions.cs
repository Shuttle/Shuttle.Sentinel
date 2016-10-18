namespace Shuttle.Sentinel
{
	public static class SystemPermissions
	{
        public static class Manage
		{
			public const string Roles = "sentinel://roles/manage";
			public const string Users = "sentinel://users/manage";
			public const string Messages = "sentinel://messages/manage";
			public const string Queues = "sentinel://queues/amange";
            public const string Subscriptions = "sentinel://subscriptions/manage";
            public const string DataStores = "sentinel://data-stores/manage";
        }

        public static class Register
		{
			public const string UserRequired = "sentinel://user/required";
		}

		public static class View
		{
			public const string Dashboard = "sentinel://dashboard/view";
		}
	}
}