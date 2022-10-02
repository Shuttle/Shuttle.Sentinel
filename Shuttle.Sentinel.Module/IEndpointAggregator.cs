using System;
using System.Collections.Generic;
using Shuttle.Sentinel.Messages.v1;

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
        void Log(DateTime dateLogged, string message);
    }
}