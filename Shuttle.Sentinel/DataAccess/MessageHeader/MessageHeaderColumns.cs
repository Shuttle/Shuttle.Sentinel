using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageHeaderColumns
    {
        public static readonly MappedColumn<string> Key = new MappedColumn<string>("Key", DbType.String);
        public static readonly MappedColumn<string> Value = new MappedColumn<string>("Value", DbType.String);
    }
}