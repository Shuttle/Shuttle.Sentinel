using System.Collections.Generic;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface ISubscriptionQuery
    {
        IEnumerable<Subscription> All();
    }
}