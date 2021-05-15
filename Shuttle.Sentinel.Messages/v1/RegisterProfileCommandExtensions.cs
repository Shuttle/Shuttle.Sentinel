using System;
using System.Net.Mail;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Messages.v1
{
    public static class RegisterProfileCommandExtensions
    {
        public static void ApplyInvariants(this RegisterProfileCommand message)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNullOrEmptyString(message.EMailAddress, nameof(message.EMailAddress));

            try
            {
                var email = new MailAddress(message.EMailAddress);
            }
            catch (FormatException ex)
            {
                throw new ApplicationException($"The value '{message.EMailAddress}' does not appear to be a valid e-mail address: {ex.Message}");
            }
        }
    }
}