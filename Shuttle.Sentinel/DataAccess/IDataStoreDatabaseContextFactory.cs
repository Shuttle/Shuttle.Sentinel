using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface IDataStoreDatabaseContextFactory : IDatabaseContextFactory
    {
        IDatabaseContext Create(Guid dataStoreId);
    }
}