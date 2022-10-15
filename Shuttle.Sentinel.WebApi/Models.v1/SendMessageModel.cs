using System.Collections.Generic;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class SendMessageModel
    {
        public class Header
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public SendMessageModel()
        {
            Headers = new List<Header>();
        }

        public string DestinationQueueUri { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public List<Header> Headers { get; set; }
    }
}