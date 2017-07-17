using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class Columns
    {
        public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
    }
}