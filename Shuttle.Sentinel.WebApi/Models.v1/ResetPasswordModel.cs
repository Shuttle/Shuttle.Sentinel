using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.WebApi.Models.v1
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