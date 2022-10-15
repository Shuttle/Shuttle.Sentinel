using System;
using System.Net.Mail;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class PasswordResetModel
    {
        public string EMailAddress { get; set; }
        
        public void ApplyInvariants()
        {
            Guard.AgainstNullOrEmptyString(EMailAddress, nameof(EMailAddress));

            try
            {
                var email = new MailAddress(EMailAddress);
            }
            catch (FormatException ex)
            {
                throw new ApplicationException($"The value '{EMailAddress}' does not appear to be a valid e-mail address: {ex.Message}");
            }
        }
    }
}