using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface ISubscriptionQueryFactory
    {
        IQuery All();
        IQuery Add(Subscription subscription);
        IQuery Remove(Subscription subscription);
    }
}