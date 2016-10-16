using System.Collections.Generic;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface IQueueQuery
    {
        void Add(string uri);
        void Remove(string uri);
        IEnumerable<Queue> All();
        IEnumerable<Queue> Search(string match);
    }
}