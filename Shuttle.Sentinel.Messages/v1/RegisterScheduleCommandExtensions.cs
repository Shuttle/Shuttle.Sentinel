using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterScheduleCommandExtensions
    {
        public static void ApplyInvariants(this RegisterScheduleCommand message)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNullOrEmptyString(message.Name, nameof(message.Name));

            try
            {
                var _ = new Uri(message.InboxWorkQueueUri);
            }
            catch
            {
                throw new Exception(string.Format(Resources.InvalidUriException, message.InboxWorkQueueUri));
            }
        }
    }
}