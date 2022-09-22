using System;
using Shuttle.Core.Contract;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel
{
    public class Profile
    {
        public Guid Id { get; }

        public Profile(Guid id)
        {
            Id = id;

            IdentityName = GetIdentityName(id);
        }

        public Registered Register(string emailAddress)
        {
            return On(new Registered
            {
                EMailAddress = emailAddress
            });
        }

        private Registered On(Registered registered)
        {
            Guard.AgainstNull(registered, nameof(registered));

            EMailAddress = registered.EMailAddress;

            return registered;
        }

        public string EMailAddress { get; private set; }
        public string IdentityName { get; }
        public bool Activated => DateActivated.HasValue;

        public DateTime? DateActivated { get; private set; }

        public static string Key(string emailAddress)
        {
            return $"[sentinel-profile]:email-address={emailAddress};";
        }

        public Activated Activate()
        {
            return On(new Activated
            {
                DateActivated = DateTime.UtcNow
            });
        }

        private Activated On(Activated activated)
        {
            Guard.AgainstNull(activated, nameof(activated));
            
            DateActivated = activated.DateActivated;

            return activated;
        }

        public PasswordResetRequested RequestPasswordReset(Guid passwordResetToken)
        {
            return On(new PasswordResetRequested
            {
                DateRequested = DateTime.UtcNow,
                PasswordResetToken = passwordResetToken
            });
        }

        private PasswordResetRequested On(PasswordResetRequested passwordResetRequested)
        {
            return passwordResetRequested;
        }

        public static string GetIdentityName(Guid id)
        {
            return $"sentinel://profile/{id}";
        }
    }
}