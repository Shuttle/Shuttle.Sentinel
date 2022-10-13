using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Shuttle.Sentinel.Module
{
    public interface IEndpointAggregator
    {
        void MessageProcessingStart(Guid messageId);
        void MessageProcessingEnd(Guid messageId, Type messageType);
        void MessageProcessingAborted(Guid messageId);
        IEnumerable<object> GetCommands();
        void RegisterAssociation(string messageTypeHandled, string messageTypeDispatched);
        void RegisterDispatched(string messageType, string recipientInboxWorkQueueUri);
        void Log(DateTime dateLogged, int logLevel, string category, int eventId, string message, string scope);
    }
}