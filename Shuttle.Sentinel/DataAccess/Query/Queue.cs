using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class Queue
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public string Processor { get; set; }
        public string Type { get; set; }
    }
}