using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class AddMessageHeader
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}