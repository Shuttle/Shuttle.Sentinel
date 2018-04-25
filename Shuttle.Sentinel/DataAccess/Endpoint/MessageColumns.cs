using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class MessageColumns
    {
        public static MappedColumn<Guid> MetricId = new MappedColumn<Guid>("MetricId", DbType.Guid);
        public static MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
        public static MappedColumn<string> MessageType = new MappedColumn<string>("MessageType", DbType.AnsiString);
        public static MappedColumn<string> MessageTypeHandled = new MappedColumn<string>("MessageTypeHandled", DbType.AnsiString);
        public static MappedColumn<string> MessageTypeDispatched = new MappedColumn<string>("MessageTypeDispatched", DbType.AnsiString);
        public static MappedColumn<string> RecipientInboxWorkQueueUri = new MappedColumn<string>("RecipientInboxWorkQueueUri", DbType.AnsiString);
        public static MappedColumn<int> Count= new MappedColumn<int>("Count", DbType.Int32);
        public static MappedColumn<double> TotalExecutionDuration = new MappedColumn<double>("TotalExecutionDuration", DbType.Double);
        public static MappedColumn<double> FastestExecutionDuration = new MappedColumn<double>("FastestExecutionDuration", DbType.Double);
        public static MappedColumn<double> SlowestExecutionDuration = new MappedColumn<double>("SlowestExecutionDuration", DbType.Double);
    }
}