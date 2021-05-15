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
        public static readonly MappedColumn<string> Match = new MappedColumn<string>("Match", DbType.AnsiString);
        public static readonly MappedColumn<Guid> PasswordResetToken = new MappedColumn<Guid>("PasswordResetToken", DbType.Guid);
        public static readonly MappedColumn<DateTime> PasswordResetTokenDateRequested = new MappedColumn<DateTime>("PasswordResetTokenDateRequested", DbType.DateTime2);
        public static readonly MappedColumn<Guid> SecurityToken = new MappedColumn<Guid>("SecurityToken", DbType.Guid);
        public static readonly MappedColumn<Guid> SentinelId = new MappedColumn<Guid>("SentinelId", DbType.Guid);
    }
}