using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeHandledQueryFactory
    {
        IQuery Search(string match);
        IQuery Register(Guid endpointId, string messageType);
    }
}