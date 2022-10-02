using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterMessageTypesHandledExtensions
    {
        public static bool HasMessageContentSizeAvailable(this RegisterMessageTypesHandled message, string messageTypeHandled, int maximumMessageContentSize)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNullOrEmptyString(messageTypeHandled, nameof(messageTypeHandled));

            return message.MessageTypesHandled.Count == 0 ||
                   message.TotalMessageContentSize() + messageTypeHandled.Length <= maximumMessageContentSize;
        }

        public static int TotalMessageContentSize(this RegisterMessageTypesHandled message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.BaseDirectory.Length +
                   message.MachineName.Length +
                   message.MessageTypesHandled.Sum(item => item.Length);
        }
    }
}