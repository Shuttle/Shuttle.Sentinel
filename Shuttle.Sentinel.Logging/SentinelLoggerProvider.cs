using Microsoft.Extensions.Logging;
using Shuttle.Core.Contract;
using Shuttle.Sentinel.Module;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace Shuttle.Sentinel.Logging
{
    public class SentinelLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, SentinelLogger> _loggers = new ConcurrentDictionary<string, SentinelLogger>();
        private readonly IEndpointAggregator _endpointAggregator;
        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;
        private SentinelLogFormatter _formatter;

        public SentinelLoggerProvider(IEndpointAggregator endpointAggregator)
        {
            Guard.AgainstNull(endpointAggregator, nameof(endpointAggregator));

            _endpointAggregator = endpointAggregator;

            _formatter = new SimpleSentinelLogFormatter(endpointAggregator);
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.TryGetValue(categoryName, out var logger) ?
            logger :
                _loggers.GetOrAdd(categoryName, new SentinelLogger(categoryName, _endpointAggregator, _formatter, _scopeProvider));
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;

            foreach (System.Collections.Generic.KeyValuePair<string, SentinelLogger> logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }
    }
}