using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class QueueColumns
    {
        public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.String, 130);
    }
}