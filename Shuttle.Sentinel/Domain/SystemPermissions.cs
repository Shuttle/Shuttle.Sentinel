namespace Shuttle.Sentinel
{
	public static class SystemPermissions
	{
		public static class Manage
		{
			public static readonly string Users = "manage://users";
		}

		public static class Register
		{
			public static readonly string InitialAdministrator = "register://initial-administrator";
			public static readonly string User = "register://user";
		}

		public static class View
		{
			public static readonly string Dashboard = "view://dashboard";
			public static readonly string Messages = "view://messages";
			public static readonly string Subscriptions = "view://subscriptions";
			public static readonly string DataStores = "view://datastores";
			public static readonly string Queues = "view://queues";
			public static readonly string Users = "view://users";
		}
	}
}