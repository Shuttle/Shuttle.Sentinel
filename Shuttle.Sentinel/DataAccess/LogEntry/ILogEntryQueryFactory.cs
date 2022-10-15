using Shuttle.Core.Data;
using System;

namespace Shuttle.Sentinel.DataAccess.LogEntry
{
    public interface ILogEntryQueryFactory
    {
        IQuery Register(Guid endpointId, DateTime dateLogged, string message, int logLevel, string category,
            int eventId, string scope);

        IQuery Search(Query.LogEntry.Specification specification);
    }
}