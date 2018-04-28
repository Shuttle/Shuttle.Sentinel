using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeDispatchedQueryFactory
    {
        IQuery Search(string match);
    }
}