using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterMessageTypeAssociationsExtensions
    {
        public static bool HasMessageContentSizeAvailable(this RegisterMessageTypeAssociations message,
            RegisterMessageTypeAssociations.Association association, int maximumMessageContentSize)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(association, nameof(association));

            return message.MessageTypeAssociations.Count == 0 ||
                   message.TotalMessageContentSize() + association.TotalMessageContentSize() <= maximumMessageContentSize;
        }

        public static int TotalMessageContentSize(this RegisterMessageTypeAssociations messageType)
        {
            Guard.AgainstNull(messageType, nameof(messageType));

            return messageType.BaseDirectory.Length +
                   messageType.MachineName.Length +
                   messageType.MessageTypeAssociations.Sum(item => item.TotalMessageContentSize());
        }

        public static int TotalMessageContentSize(this RegisterMessageTypeAssociations.Association message)
        {
            Guard.AgainstNull(message, nameof(message));

            return message.MessageTypeHandled.Length +
                   message.MessageTypeDispatched.Length;
        }
    }
}