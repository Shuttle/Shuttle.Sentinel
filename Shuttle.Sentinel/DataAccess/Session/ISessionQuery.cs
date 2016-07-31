using System;

namespace Shuttle.Sentinel
{
    public interface ISessionQuery
    {
        bool Contains(Guid token);
        bool Contains(Guid token, string permission);
    }
}