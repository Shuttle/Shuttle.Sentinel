using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.DataAccess.Profile
{
    public interface IProfileQuery
    {
        IEnumerable<Query.Profile> Search(Query.Profile.Specification specification);
        void RegisterSecurityToken(string emailAddress, Guid securityToken);
        void RemoveSecurityToken(Guid securityToken);
        bool Contains(Query.Profile.Specification specification);
    }
}