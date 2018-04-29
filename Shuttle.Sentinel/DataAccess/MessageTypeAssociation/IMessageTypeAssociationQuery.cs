using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeAssociationQuery
    {
        IEnumerable<MessageTypeAssociation> Search(string match);
    }
}