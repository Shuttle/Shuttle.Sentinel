using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterMessageTypesDispatchedExtensions
    {
        public static bool HasMessageContentSizeAvailable(this RegisterMessageTypesDispatched message,
            RegisterMessageTypesDispatched.Dispatched dispatched, int maximumMessageContentSize)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(dispatched, nameof(dispatched));

            return message.MessageTypesDispatched.Count == 0 ||
                   message.TotalMessageContentSize() + dispatched.TotalMessageContentSize() <= maximumMessageContentSize;
        }

        public static int TotalMessageContentSize(this RegisterMessageTypesDispatched message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.BaseDirectory.Length +
                   message.MachineName.Length +
                   message.MessageTypesDispatched.Sum(item => item.TotalMessageContentSize());
        }

        public static int TotalMessageContentSize(this RegisterMessageTypesDispatched.Dispatched message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.MessageType.Length +
                   message.RecipientInboxWorkQueueUri.Length;
        }
    }
}