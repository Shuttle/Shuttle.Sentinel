using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Logging
{
    public abstract class SentinelLogFormatter
    {
        protected SentinelLogFormatter(string name)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));

            Name = name;
        }

        public string Name { get; }

        public abstract void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider);
    }
}
