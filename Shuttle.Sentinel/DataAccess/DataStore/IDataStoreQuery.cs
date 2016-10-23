using System;
using System.Collections.Generic;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface IDataStoreQuery
    {
        void Add(DataStore dataStore);
        void Remove(string name);
        IEnumerable<DataStore> All();
        void Edit(DataStore dataStore);
    }
}