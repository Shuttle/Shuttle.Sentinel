namespace Shuttle.Sentinel
{
	public static class SystemPermissions
	{
		public static class Manage
		{
			public const string Users = "manage://users";
		}

		public static class Register
		{
			public const string InitialAdministrator = "register://initial-administrator";
			public const string User = "register://user";
		}

		public static class View
		{
			public const string Dashboard = "view://dashboard";
			public const string Messages = "view://messages";
			public const string Subscriptions = "view://subscriptions";
			public const string DataStores = "view://datastores";
			public const string Queues = "view://queues";
			public const string Users = "view://users";
		}
	}
}