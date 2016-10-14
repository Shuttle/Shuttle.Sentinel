using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface IQueueQueryFactory
    {
        IQuery Add(string uri);
        IQuery Remove(string uri);
        IQuery All();
    }
}