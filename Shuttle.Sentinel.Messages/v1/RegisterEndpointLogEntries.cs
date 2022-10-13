 using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterEndpointLogEntries : EndpointMessage
    {
        public List<LogEntry> LogEntries { get; set; } = new List<LogEntry>();

        public class LogEntry
        {
            public DateTime DateLogged { get; set; }
            public string Message { get; set; }
            public int LogLevel { get; set; }
            public string Category { get; set; }
            public int EventId { get; set; }
            public string Scope { get; set; }
        }
    }
}
