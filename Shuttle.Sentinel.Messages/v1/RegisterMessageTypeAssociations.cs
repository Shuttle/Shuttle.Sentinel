using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypeAssociations : EndpointMessage
    {
        public List<Association> MessageTypeAssociations { get; set; } = new List<Association>();

        public class Association
        {
            public string MessageTypeHandled { get; set; }
            public string MessageTypeDispatched { get; set; }
        }
    }
}
