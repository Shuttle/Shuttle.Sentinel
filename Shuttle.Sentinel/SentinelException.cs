using System;
using System.Runtime.Serialization;

namespace Shuttle.Sentinel
{
    public class SentinelException : Exception
    {
        public SentinelException()
        {
        }

        public SentinelException(string message) : base(message)
        {
        }

        public SentinelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SentinelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}