using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterEndpointLogEntriesExtensions
    {
        public static bool HasMessageContentSizeAvailable(this RegisterEndpointLogEntries message, RegisterEndpointLogEntries.LogEntry logEntry, int maximumMessageContentSize)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(logEntry, nameof(logEntry));

            return message.LogEntries.Count == 0 ||
                   message.TotalMessageContentSize() + logEntry.TotalMessageContentSize() <= maximumMessageContentSize;
        }

        public static int TotalMessageContentSize(this RegisterEndpointLogEntries message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.BaseDirectory.Length +
                   message.MachineName.Length +
                   message.LogEntries.Sum(item => item.TotalMessageContentSize());
        }

        public static int TotalMessageContentSize(this RegisterEndpointLogEntries.LogEntry message)
        {
            Guard.AgainstNull(message, nameof(message));

            return 8 + message.Message.Length;
        }
    }
}