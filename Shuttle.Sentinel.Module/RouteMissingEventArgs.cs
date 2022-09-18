using System;

namespace Shuttle.Sentinel.Module
{
    public class RouteMissingEventArgs : System.EventArgs
    {
        public RouteMissingEventArgs(Type messageType)
        {
        }
    }
}