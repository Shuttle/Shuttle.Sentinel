using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Logging
{
    public static class SentinelLoggerExtensions
    {
        public static ILoggingBuilder AddSentinelLogger(this ILoggingBuilder builder)
        {
            Guard.AgainstNull(builder, nameof(builder));

            builder.AddConfiguration();

            builder.Services.TryAddSingleton<SentinelLogFormatter, SimpleSentinelLogFormatter>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SentinelLoggerProvider>());

            return builder;
        }
    }
}