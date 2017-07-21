using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface ISentinelDatabaseContextFactory : IDatabaseContextFactory
    {
        IDatabaseContext Create(Guid dataStoreId);
    }
}