using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class QueueColumns
    {
        public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.String, 130);
        public static readonly MappedColumn<string> DisplayUri = new MappedColumn<string>("DisplayUri", DbType.String, 130);
    }
}