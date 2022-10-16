using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;
using System;

namespace Shuttle.Sentinel.DataAccess
{
    public interface ISubscriptionQueryFactory
    {
        IQuery All();
        IQuery Register(Guid endpointId, string messageType);
    }
}