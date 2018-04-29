using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class Columns
    {
        public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
        public static readonly MappedColumn<DateTime> Date = new MappedColumn<DateTime>("Date", DbType.DateTime);
        public static readonly MappedColumn<string> Match = new MappedColumn<string>("Match", DbType.AnsiString);
    }
}