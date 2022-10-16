using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterSubscriptions : EndpointMessage
    {
        public List<string> MessageTypes { get; set; } = new List<string>();
    }
}
