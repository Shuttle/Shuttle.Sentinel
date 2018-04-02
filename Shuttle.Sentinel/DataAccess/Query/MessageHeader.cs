using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class MessageHeader
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}