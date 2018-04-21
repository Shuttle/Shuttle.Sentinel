using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IQueueQuery
    {
        void Save(string uri, string processor, string type);
        void Remove(Guid id);
        IEnumerable<Queue> All();
        IEnumerable<Queue> Search(string match);
    }
}