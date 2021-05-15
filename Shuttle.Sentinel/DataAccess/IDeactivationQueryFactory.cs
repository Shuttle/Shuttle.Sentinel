using System;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IDeactivationQueryFactory
    {
        string Deactivate(string table, Guid id);
    }
}