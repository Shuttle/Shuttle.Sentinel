using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class DataStoreColumns
    {
        public static readonly MappedColumn<string> Name = new MappedColumn<string>("Name", DbType.AnsiString, 64);

        public static readonly MappedColumn<string> ConnectionString = new MappedColumn<string>("ConnectionString",
            DbType.AnsiString, 1024);

        public static readonly MappedColumn<string> ProviderName = new MappedColumn<string>("ProviderName",
            DbType.AnsiString, 512);
    }
}