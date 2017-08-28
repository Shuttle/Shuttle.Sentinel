using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeAssociationHandler : IMessageHandler<RegisterMessageTypeAssociationCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMessageTypeAssociationCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}