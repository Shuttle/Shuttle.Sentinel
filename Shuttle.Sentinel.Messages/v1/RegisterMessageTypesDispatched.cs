using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypesDispatched : EndpointMessage
    {
        public List<Dispatched> MessageTypesDispatched { get; set; } = new List<Dispatched>();

        public class Dispatched
        {
            public string MessageType { get; set; }
            public string RecipientInboxWorkQueueUri { get; set; }
        }
    }
}
