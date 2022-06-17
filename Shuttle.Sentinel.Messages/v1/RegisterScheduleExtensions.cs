using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterScheduleExtensions
    {
        public static void ApplyInvariants(this RegisterSchedule message)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNullOrEmptyString(message.Name, nameof(message.Name));

            try
            {
                _ = new Uri(message.InboxWorkQueueUri);
            }
            catch
            {
                throw new Exception(string.Format(Resources.InvalidUriException, message.InboxWorkQueueUri));
            }
        }
    }
}