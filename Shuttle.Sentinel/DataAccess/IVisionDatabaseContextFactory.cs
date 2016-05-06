using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public interface IVisionDatabaseContextFactory
	{
		IDatabaseContext Create();
	}
}