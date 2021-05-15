using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.WebApi.Models
{
    public class ResetPasswordModel
    {
        public Guid PasswordResetToken { get; set; }
        public string Password { get; set; }

        public void ApplyInvariants()
        {
            Guard.AgainstNullOrEmptyString(Password, nameof(Password));
        }
    }
}