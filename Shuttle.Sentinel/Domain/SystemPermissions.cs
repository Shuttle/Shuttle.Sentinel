namespace Shuttle.Sentinel
{
	public static class SystemPermissions
	{
        public static class Add
        {
            public const string Role = "sentinel://role/add";
        }

        public static class Manage
		{
			public const string Roles = "sentinel://roles/manage";
			public const string Users = "sentinel://users/manage";
		}

		public static class Register
		{
			public const string UserRequired = "sentinel://user/required";
			public const string User = "sentinel://user/register";
		}

		public static class View
		{
			public const string Dashboard = "sentinel://dashboard/view";
			public const string Messages = "sentinel://messages/view";
			public const string Subscriptions = "sentinel://subscriptions/view";
			public const string DataStores = "sentinel://data-stores/view";
			public const string Queues = "sentinel://queues/view";
			public const string Roles = "sentinel://roles/view";
			public const string Users = "sentinel://users/view";
		}
	}
}