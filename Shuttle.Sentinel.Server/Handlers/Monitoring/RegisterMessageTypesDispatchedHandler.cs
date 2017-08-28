using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypesDispatchedHandler : IMessageHandler<RegisterMessageTypesDispatchedCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMessageTypesDispatchedCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}