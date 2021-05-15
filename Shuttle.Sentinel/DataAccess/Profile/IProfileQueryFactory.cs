using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.Events.Profile.v1;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public interface IProfileQueryFactory
    {
        IQuery Register(Guid primitiveEventId, Registered domainEvent);
        IQuery Search(Query.Profile.Specification specification);
        IQuery PasswordResetToken(Guid primitiveEventId, PasswordResetRequested domainEvent);
        IQuery RegisterSecurityToken(string emailAddress, Guid securityToken);
        IQuery RemoveSecurityToken(Guid securityToken);
    }
}