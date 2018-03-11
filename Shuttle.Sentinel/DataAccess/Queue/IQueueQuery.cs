using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IQueueQuery
    {
        void Add(string uri, string displayUri);
        void Remove(Guid id);
        IEnumerable<Queue> All();
        IEnumerable<Queue> Search(string match);
    }
}