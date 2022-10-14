using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeAssociationQueryFactory
    {
        IQuery Search(string match);
        IQuery Register(Guid endpointId, string messageTypeHandled, string messageTypeDispatched);
    }
}