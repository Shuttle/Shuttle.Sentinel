using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.DataAccess.LogEntry
{
    public interface ILogEntryQuery
    {
        IEnumerable<Query.LogEntry> Search(Query.LogEntry.Specification specification);
        void Register(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
            int eventId, string scope);
    }
}