﻿namespace Shuttle.Sentinel.DataAccess.Query
{
    public class MessageTypeDispatched
    {
        public string EnvironmentName { get; set; }
        public string MessageType { get; set; }
        public string RecipientInboxWorkQueueUri { get; set; }
        public int EndpointCount { get; set; }
    }
}