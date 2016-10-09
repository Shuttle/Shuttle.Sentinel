using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.WebApi
{
    public class MessageMoveModel
    {
        public List<Guid> MessageIds { get; set; }
        public string DestinationQueueUri { get; set; }
        public string Action { get; set; }
    }
}