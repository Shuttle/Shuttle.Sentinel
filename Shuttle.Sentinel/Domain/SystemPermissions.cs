namespace Shuttle.Sentinel
{
	public static class SystemPermissions
	{
		public static class Management
		{
			public static readonly string Users = "management://users";
		}

		public static class Register
		{
			public static readonly string User = "register://user";
		}

		public static class Roles
		{
			public static readonly string Administrator = "roles://administrator";
		}

		public static class States
		{
			public static readonly string UserRequired = "states://user-required";
		}

		public static class View
		{
			public static readonly string Dashboard = "view://dashboard";
		}
	}
}