using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterQueue
    {
        public Guid? Id { get; set; }
        public string Uri { get; set; }
        public string Processor { get; set; }
        public string Type { get; set; }
    }
}