using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeDispatchedQueryFactory
    {
        IQuery Search(string match);
        IQuery Register(Guid endpointId, string dispatchedMessageType, string recipientInboxWorkQueueUri);
    }
}