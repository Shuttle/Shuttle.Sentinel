using System;
using System.Collections.Generic;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface IDataStoreQuery
    {
        void Add(DataStore dataStore);
        void Remove(Guid id);
        IEnumerable<DataStore> All();
        DataStore Get(Guid id);
    }
}