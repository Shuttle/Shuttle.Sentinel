using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class QueueColumns
    {
        public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.String);
    }
}