using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public class SessionMapper : IDataRowMapper<Session>
	{
		public MappedRow<Session> Map(DataRow row)
		{
			return new MappedRow<Session>(row, new Session(
				SessionColumns.Token.MapFrom(row), 
				SessionColumns.EMail.MapFrom(row),
				SessionColumns.DateRegistered.MapFrom(row)));
		}
	}
}