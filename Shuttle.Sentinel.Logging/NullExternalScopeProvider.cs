using Microsoft.Extensions.Logging;
using System;

namespace Shuttle.Sentinel.Logging
{
    internal sealed class NullExternalScopeProvider : IExternalScopeProvider
    {
        private NullExternalScopeProvider()
        {
        }

        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();

        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
        }

        IDisposable IExternalScopeProvider.Push(object state)
        {
            return NullScope.Instance;
        }
    }
}
