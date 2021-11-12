using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterQueueCommand
    {
        public Guid? Id { get; set; }
        public string Uri { get; set; }
        public string Processor { get; set; }
        public string Type { get; set; }
    }
}