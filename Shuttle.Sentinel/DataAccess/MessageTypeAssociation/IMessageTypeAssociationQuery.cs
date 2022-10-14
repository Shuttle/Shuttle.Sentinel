using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeAssociationQuery
    {
        IEnumerable<MessageTypeAssociation> Search(string match);
        void Register(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);
    }
}