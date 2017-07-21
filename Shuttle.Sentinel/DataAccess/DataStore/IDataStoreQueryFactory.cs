using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface IDataStoreQueryFactory
    {
        IQuery Add(DataStore dataStore);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Edit(DataStore dataStore);
    }
}