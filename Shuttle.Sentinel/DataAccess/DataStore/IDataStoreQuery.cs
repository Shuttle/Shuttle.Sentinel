using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IDataStoreQuery
    {
        void Add(DataStore dataStore);
        void Remove(Guid id);
        IEnumerable<DataStore> Search(DataStore.Specification specification);
        DataStore Get(Guid id);
    }
}