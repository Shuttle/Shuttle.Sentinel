using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IDataStoreQueryFactory
    {
        IQuery Add(DataStore dataStore);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Get(Guid id);
    }
}