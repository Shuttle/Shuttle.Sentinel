using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class QueueColumns
    {
        public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.String);
        public static readonly MappedColumn<string> Processor = new MappedColumn<string>("Processor", DbType.String);
        public static readonly MappedColumn<string> Type = new MappedColumn<string>("Type", DbType.String);
    }
}