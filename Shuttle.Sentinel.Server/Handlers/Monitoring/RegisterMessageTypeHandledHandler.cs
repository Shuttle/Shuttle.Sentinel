using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeHandledHandler : IMessageHandler<RegisterMessageTypeHandledCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMessageTypeHandledCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}