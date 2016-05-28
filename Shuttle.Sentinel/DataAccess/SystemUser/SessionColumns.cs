using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public class SystemUserColumns
	{
		public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
		public static readonly MappedColumn<string> EMail = new MappedColumn<string>("EMail", DbType.String, 254);
		public static readonly MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
		public static readonly MappedColumn<string> RegisteredBy = new MappedColumn<string>("RegisteredBy", DbType.String, 65);
		public static readonly MappedColumn<DateTime> DateActivated = new MappedColumn<DateTime>("DateActivated", DbType.DateTime);
	}
}