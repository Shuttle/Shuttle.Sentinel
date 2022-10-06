using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class Columns
    {
        public static readonly MappedColumn<DateTime> Date = new MappedColumn<DateTime>("Date", DbType.DateTime);
        public static readonly MappedColumn<DateTime> DateTimeMaxValue = new MappedColumn<DateTime>("DateTimeMaxValue", DbType.DateTime2);
        public static readonly MappedColumn<DateTime> EffectiveDate = new MappedColumn<DateTime>("EffectiveDate", DbType.DateTime2);
        public static readonly MappedColumn<DateTime> EffectiveFromDate = new MappedColumn<DateTime>("EffectiveFromDate", DbType.DateTime2);
        public static readonly MappedColumn<DateTime> EffectiveToDate = new MappedColumn<DateTime>("EffectiveToDate", DbType.DateTime2);
        public static readonly MappedColumn<string> EMailAddress = new MappedColumn<string>("EMailAddress", DbType.AnsiString);
        public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
        public static readonly MappedColumn<Guid> OwnerId = new MappedColumn<Guid>("OwnerId", DbType.Guid);
        public static readonly MappedColumn<string> Match = new MappedColumn<string>("Match", DbType.AnsiString);
        public static readonly MappedColumn<Guid> PasswordResetToken = new MappedColumn<Guid>("PasswordResetToken", DbType.Guid);
        public static readonly MappedColumn<DateTime> PasswordResetTokenDateRequested = new MappedColumn<DateTime>("PasswordResetTokenDateRequested", DbType.DateTime2);
        public static readonly MappedColumn<Guid> SecurityToken = new MappedColumn<Guid>("SecurityToken", DbType.Guid);
        public static readonly MappedColumn<Guid> SentinelId = new MappedColumn<Guid>("SentinelId", DbType.Guid);
        public static readonly MappedColumn<string> MachineName = new MappedColumn<string>("MachineName", DbType.AnsiString);
        public static readonly MappedColumn<string> BaseDirectory = new MappedColumn<string>("BaseDirectory", DbType.AnsiString);
        public static readonly MappedColumn<string> EntryAssemblyQualifiedName = new MappedColumn<string>("EntryAssemblyQualifiedName", DbType.AnsiString);
        public static readonly MappedColumn<string> IPv4Address = new MappedColumn<string>("IPv4Address", DbType.AnsiString);
        public static readonly MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> InboxErrorQueueUri = new MappedColumn<string>("InboxErrorQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> InboxDeferredQueueUri = new MappedColumn<string>("InboxDeferredQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> ControlInboxWorkQueueUri = new MappedColumn<string>("ControlInboxWorkQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> ControlInboxErrorQueueUri = new MappedColumn<string>("ControlInboxErrorQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> OutboxWorkQueueUri = new MappedColumn<string>("OutboxWorkQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<string> OutboxErrorQueueUri = new MappedColumn<string>("OutboxErrorQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<DateTime> HeartbeatDate = new MappedColumn<DateTime>("HeartbeatDate", DbType.DateTime);
        public static readonly MappedColumn<string> HeartbeatIntervalDuration = new MappedColumn<string>("HeartbeatIntervalDuration", DbType.AnsiString);
        public static readonly MappedColumn<Guid> MetricId = new MappedColumn<Guid>("MetricId", DbType.Guid);
        public static readonly MappedColumn<DateTime> DateLogged= new MappedColumn<DateTime>("DateLogged", DbType.DateTime);
        public static readonly MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
        public static readonly MappedColumn<string> MessageType = new MappedColumn<string>("MessageType", DbType.AnsiString);
        public static readonly MappedColumn<string> MessageTypeHandled = new MappedColumn<string>("MessageTypeHandled", DbType.AnsiString);
        public static readonly MappedColumn<string> MessageTypeDispatched = new MappedColumn<string>("MessageTypeDispatched", DbType.AnsiString);
        public static readonly MappedColumn<string> RecipientInboxWorkQueueUri = new MappedColumn<string>("RecipientInboxWorkQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<int> Count = new MappedColumn<int>("Count", DbType.Int32);
        public static readonly MappedColumn<double> TotalExecutionDuration = new MappedColumn<double>("TotalExecutionDuration", DbType.Double);
        public static readonly MappedColumn<double> FastestExecutionDuration = new MappedColumn<double>("FastestExecutionDuration", DbType.Double);
        public static readonly MappedColumn<double> SlowestExecutionDuration = new MappedColumn<double>("SlowestExecutionDuration", DbType.Double);
        public static readonly MappedColumn<string> SourceQueueUri = new MappedColumn<string>("SourceQueueUri", DbType.AnsiString);
        public static readonly MappedColumn<Guid> MessageId = new MappedColumn<Guid>("MessageId", DbType.Guid);
        public static readonly MappedColumn<byte[]> MessageBody = new MappedColumn<byte[]>("MessageBody", DbType.Binary);
        public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.String);
        public static readonly MappedColumn<string> Processor = new MappedColumn<string>("Processor", DbType.String);
        public static readonly MappedColumn<string> Type = new MappedColumn<string>("Type", DbType.String);
        public static readonly MappedColumn<string> Message = new MappedColumn<string>("Message", DbType.String);
        public static readonly MappedColumn<string> Tag = new MappedColumn<string>("Tag", DbType.String);
        public static readonly MappedColumn<string> Name = new MappedColumn<string>("Name", DbType.String);
        public static readonly MappedColumn<decimal> Value = new MappedColumn<decimal>("Value", DbType.Decimal);
        public static readonly MappedColumn<DateTime> DateStarted = new MappedColumn<DateTime>("DateStarted", DbType.DateTime);
        public static readonly MappedColumn<DateTime> DateStopped = new MappedColumn<DateTime>("DateStopped", DbType.DateTime);
    }
}