using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
	public class SessionColumns
	{
		public static readonly MappedColumn<Guid> Token = new MappedColumn<Guid>("Token", DbType.Guid);
		public static readonly MappedColumn<string> EMail = new MappedColumn<string>("EMail", DbType.String, 65);
		public static readonly MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
	}
}