using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.InspectionQueue
{
    public class InspectionQueueColumns
    {
        public static MappedColumn<Guid> MessageId = new MappedColumn<Guid>("MessageId", DbType.Guid);
        public static MappedColumn<byte[]> MessageBody = new MappedColumn<byte[]>("MessageBody", DbType.Binary);
    }
}