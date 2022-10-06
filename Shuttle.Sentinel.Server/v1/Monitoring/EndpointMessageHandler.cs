using System;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Server;

public abstract class EndpointMessageHandler
{
    protected void Defer(IHandlerContext context, object message)
    {
        context.Send(message, builder =>
        {
            builder.Local();
            builder.Defer(DateTime.UtcNow.AddSeconds(5));
        });
    }
}