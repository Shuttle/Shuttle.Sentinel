using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypesHandledQueryFactory
    {
        IQuery Search(string match);
    }
}