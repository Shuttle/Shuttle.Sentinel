using System;

namespace Shuttle.Sentinel.Events.Profile.v1
{
    public class PasswordResetRequested
    {
        public DateTime DateRequested { get; set; }
        public Guid PasswordResetToken { get; set; }
    }
}