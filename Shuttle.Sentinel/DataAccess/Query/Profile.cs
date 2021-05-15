using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid SentinelId { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public string EMailAddress { get; set; }
        public DateTime? DateActivated { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenDateRequested { get; set; }

        public class Specification
        {
            public Guid? SentinelId { get; private set; }
            public string EMailAddress { get; private set; }
            public DateTime? EffectiveDate { get; private set; }
            public Guid? PasswordResetToken { get; private set; }
            public Guid? SecurityToken { get; private set; }

            public Specification WithSentinelId(Guid sentinelId)
            {
                SentinelId = sentinelId;

                return this;
            }

            public Specification WithEMailAddress(string emailAddress)
            {
                Guard.AgainstNullOrEmptyString(emailAddress, nameof(emailAddress));

                EMailAddress = emailAddress;

                return this;
            }

            public Specification WithEffectiveDate(DateTime effectiveDate)
            {
                EffectiveDate = effectiveDate;

                return this;
            }

            public Specification WithPasswordResetToken(Guid passwordResetToken)
            {
                PasswordResetToken = passwordResetToken;

                return this;
            }

            public Specification WithSecurityToken(Guid securityToken)
            {
                SecurityToken = securityToken;

                return this;
            }
        }
    }
}