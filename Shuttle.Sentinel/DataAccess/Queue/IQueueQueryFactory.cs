using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface IQueueQueryFactory
    {
        IQuery Add(string uri, string displayUri);
        IQuery Remove(string uri);
        IQuery All();
        IQuery Search(string match);
    }
}