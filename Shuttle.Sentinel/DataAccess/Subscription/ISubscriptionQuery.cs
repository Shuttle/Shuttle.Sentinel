using System.Collections.Generic;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface ISubscriptionQuery
    {
        IEnumerable<Subscription> All();
        void Add(Subscription subscription);
        void Remove(Subscription subscription);
    }
}