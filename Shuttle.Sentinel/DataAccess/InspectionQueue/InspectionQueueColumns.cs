using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public class InspectionQueueColumns
    {
        public static MappedColumn<string> SourceQueueUri = new MappedColumn<string>("SourceQueueUri", DbType.AnsiString);
        public static MappedColumn<Guid> MessageId = new MappedColumn<Guid>("MessageId", DbType.Guid);
        public static MappedColumn<byte[]> MessageBody = new MappedColumn<byte[]>("MessageBody", DbType.Binary);
    }
}