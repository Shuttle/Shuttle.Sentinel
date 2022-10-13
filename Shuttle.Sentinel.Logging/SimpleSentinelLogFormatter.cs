using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shuttle.Core.Contract;
using Shuttle.Sentinel.Module;

namespace Shuttle.Sentinel.Logging
{
    public class SimpleSentinelLogFormatter : SentinelLogFormatter
    {
        private const string LogLevelPadding = ": ";
        private readonly IEndpointAggregator _endpointAggregator;

        public SimpleSentinelLogFormatter(IEndpointAggregator endpointAggregator) : base("simple")
        {
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));

            _endpointAggregator = endpointAggregator;
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider)
        {
            Guard.AgainstNull(logEntry, nameof(logEntry));

            var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);

            if (logEntry.Exception == null && message == null)
            {
                return;
            }

            var now = DateTime.UtcNow;
            var scopes = new StringBuilder();

            scopeProvider?.ForEachScope((scope, state) =>
            {
                state.Append($"{(state.Length > 0 ? "\\" : string.Empty)}{scope.ToString().Replace("\\", "-")}");
            }, scopes);

            _endpointAggregator.Log(now, (int)logEntry.LogLevel, logEntry.Category, logEntry.EventId.Id, message,
                scopes.ToString());
        }
    }
}