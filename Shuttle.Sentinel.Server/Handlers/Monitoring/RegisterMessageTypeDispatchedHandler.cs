using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeDispatchedHandler : IMessageHandler<RegisterMessageTypeDispatchedCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMessageTypeDispatchedCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}