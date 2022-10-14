using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeHandledQuery
    {
        IEnumerable<MessageTypeHandled> Search(string match);
        void Register(Guid endpointId, string messageType);
    }
}