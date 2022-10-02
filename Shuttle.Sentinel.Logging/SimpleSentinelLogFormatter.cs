using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shuttle.Core.Contract;
using Shuttle.Sentinel.Module;
using System.IO;
using System;

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

            var sw = new StringWriter();
            var now = DateTime.UtcNow;

            sw.Write($"{now:O} GetLogLevelString(logLevel)");

            CreateDefaultLogMessage(sw, logEntry, message, scopeProvider);

            _endpointAggregator.Log(now, sw.GetStringBuilder().ToString());
        }

        private void CreateDefaultLogMessage<TState>(TextWriter textWriter, in LogEntry<TState> logEntry, string message, IExternalScopeProvider scopeProvider)
        {
            var eventId = logEntry.EventId.Id;
            var exception = logEntry.Exception;

            textWriter.Write(LogLevelPadding);
            textWriter.Write(logEntry.Category);
            textWriter.Write('[');
            textWriter.Write(eventId.ToString());
            textWriter.Write(']');

            WriteScopeInformation(textWriter, scopeProvider);
            WriteMessage(textWriter, message);

            if (exception != null)
            {
                WriteMessage(textWriter, exception.ToString());
            }
        }

        private static void WriteMessage(TextWriter textWriter, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            textWriter.Write(' ');
            WriteReplacing(textWriter, Environment.NewLine, " ", message);
        }

        private static void WriteReplacing(TextWriter writer, string oldValue, string newValue, string message)
        {
            writer.Write(message.Replace(oldValue, newValue));
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    {
                        return "trce";
                    }
                case LogLevel.Debug:
                    {
                        return "dbug";
                    }
                case LogLevel.Information:
                    {
                        return "info";
                    }
                case LogLevel.Warning:
                    {
                        return "warn";
                    }
                case LogLevel.Error:
                    {
                        return "fail";
                    }
                case LogLevel.Critical:
                    {
                        return "crit";
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(logLevel));
                    }
            }
        }

        private void WriteScopeInformation(TextWriter textWriter, IExternalScopeProvider scopeProvider)
        {
            if (scopeProvider == null)
            {
                return;
            }

            scopeProvider.ForEachScope((scope, state) =>
            {
                state.Write(" => ");
                state.Write(scope);
            }, textWriter);
        }
    }
}
