using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface ISubscriptionQuery
    {
        IEnumerable<Subscription> All();
        void Register(Guid endpointId, string messageType);
    }
}