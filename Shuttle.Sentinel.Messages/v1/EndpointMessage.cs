using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public abstract class EndpointMessage
    {
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }

        public static T Create<T>() where T : EndpointMessage, new()
        {
            return new T
            {
                MachineName = Environment.MachineName,
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
        }
    }
}