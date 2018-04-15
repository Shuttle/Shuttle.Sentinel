using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class ServerColumns
    {
        public static MappedColumn<string> MachineName = new MappedColumn<string>("MachineName", DbType.AnsiString);
        public static MappedColumn<string> BaseDirectory = new MappedColumn<string>("BaseDirectory", DbType.AnsiString);
        public static MappedColumn<string> IPv4Address = new MappedColumn<string>("IPv4Address", DbType.AnsiString);
        public static MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.AnsiString);
        public static MappedColumn<string> InboxErrorQueueUri = new MappedColumn<string>("InboxErrorQueueUri", DbType.AnsiString);
        public static MappedColumn<string> InboxDeferredQueueUri = new MappedColumn<string>("InboxDeferredQueueUri", DbType.AnsiString);
        public static MappedColumn<string> ControlInboxWorkQueueUri = new MappedColumn<string>("ControlInboxWorkQueueUri", DbType.AnsiString);
        public static MappedColumn<string> ControlInboxErrorQueueUri = new MappedColumn<string>("ControlInboxErrorQueueUri", DbType.AnsiString);
        public static MappedColumn<string> OutboxWorkQueueUri = new MappedColumn<string>("OutboxWorkQueueUri", DbType.AnsiString);
        public static MappedColumn<string> OutboxErrorQueueUri = new MappedColumn<string>("OutboxErrorQueueUri", DbType.AnsiString);
    }
}