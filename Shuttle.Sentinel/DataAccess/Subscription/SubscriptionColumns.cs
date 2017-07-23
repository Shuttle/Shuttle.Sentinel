using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class SubscriptionColumns
    {
        public static readonly MappedColumn<string> MessageType = new MappedColumn<string>("MessageType", DbType.String);
        public static readonly MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.String);
    }
}