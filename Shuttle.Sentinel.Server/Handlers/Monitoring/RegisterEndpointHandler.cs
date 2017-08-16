using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterEndpointHandler : IMessageHandler<RegisterEndpointCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterEndpointCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}