using Shuttle.Core.Data;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public interface ISubscriptionQueryFactory
    {
        IQuery All();
        IQuery Add(Subscription subscription);
        IQuery Remove(Subscription subscription);
    }
}