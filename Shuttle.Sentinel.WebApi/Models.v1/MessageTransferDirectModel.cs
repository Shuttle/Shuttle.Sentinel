﻿namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class MessageTransferDirectModel
    {
        public string SourceQueueUri { get; set; }
        public string DestinationQueueUri { get; set; }
        public string Action { get; set; }
        public int Count { get; set; }
    }
}