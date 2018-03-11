using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IDataStoreDatabaseContextFactory : IDatabaseContextFactory
    {
        IDatabaseContext Create(Guid dataStoreId);
    }
}