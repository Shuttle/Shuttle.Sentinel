using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.Module
{
    public class RouteMissingEventArgs : EventArgs
    {
        public RouteMissingEventArgs(Type messageType)
        {
            Guard.AgainstNull(messageType, nameof(messageType));

            MessageType = messageType;
        }

        public Type MessageType { get; }
    }
}