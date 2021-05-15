using Shuttle.Recall;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public interface IProfileProjectionQuery
    {
        void Register(PrimitiveEvent primitiveEvent, Registered domainEvent);
        void PasswordResetToken(PrimitiveEvent primitiveEvent, PasswordResetRequested domainEvent);
    }
}