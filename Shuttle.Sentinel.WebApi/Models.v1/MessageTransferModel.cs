using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class MessageTransferModel
    {
        public List<Guid> MessageIds { get; set; }
        public string DestinationQueueUri { get; set; }
        public string Action { get; set; }
    }
}