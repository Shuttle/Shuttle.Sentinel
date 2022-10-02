using System;
using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterMessageTypeMetricsExtensions
    {
        public static bool HasMessageContentSizeAvailable(this RegisterMessageTypeMetrics message,
            RegisterMessageTypeMetrics.MessageTypeMetric metric, int maximumMessageContentSize)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(metric, nameof(metric));

            return message.MessageTypeMetrics.Count == 0 ||
                   message.TotalMessageContentSize() + metric.TotalMessageContentSize() <= maximumMessageContentSize;
        }

        public static int TotalMessageContentSize(this RegisterMessageTypeMetrics message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.BaseDirectory.Length +
                   message.MachineName.Length +
                   16 + // 2 * DateTime
                   message.MessageTypeMetrics.Sum(item => item.TotalMessageContentSize());
        }

        public static int TotalMessageContentSize(this RegisterMessageTypeMetrics.MessageTypeMetric message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.MessageType.Length +
                   sizeof(int) +
                   sizeof(double) * 3;
        }
    }
}