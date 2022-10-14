using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeDispatchedQuery
    {
        IEnumerable<MessageTypeDispatched> Search(string match);
        void Register(Guid endpointId, string messageType, string recipientInboxWorkQueueUri);
    }
}