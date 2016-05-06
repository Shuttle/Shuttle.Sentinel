using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public class VisionDatabaseContextFactory : IVisionDatabaseContextFactory
	{
		private readonly IDatabaseContextFactory _databaseContextFactory;
		private readonly string _providerName;
		private readonly string _connectionString;

		public VisionDatabaseContextFactory(IDatabaseContextFactory databaseContextFactory, IVisionConfiguration configuration)
		{
			Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
			Guard.AgainstNull(configuration, "configuration");

			_databaseContextFactory = databaseContextFactory;
			_providerName = configuration.ProviderName;
			_connectionString = configuration.ConnectionString;
		}

		public IDatabaseContext Create()
		{
			return _databaseContextFactory.Create(_providerName, _connectionString);
		}
	}
}