using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Logging
{
    public class SentinelLogger : ILogger
    {
        private readonly string _name;
        private readonly SentinelLogFormatter _formatter;

        public SentinelLogger(string name, SentinelLogFormatter formatter, IExternalScopeProvider scopeProvider)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));

            _name = name;
            _formatter = formatter;
            
            ScopeProvider = scopeProvider;
        }

        public IExternalScopeProvider ScopeProvider { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Guard.AgainstNull(formatter, nameof(formatter));

            if (!IsEnabled(logLevel))
            {
                return;
            }

            var logEntry = new LogEntry<TState>(logLevel, _name, eventId, state, exception, formatter);
            
            _formatter.Write(in logEntry, ScopeProvider);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state) ?? NullScope.Instance;
        }
    }
}
