using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeAssociationQueryFactory
    {
        IQuery Search(string match);
    }
}