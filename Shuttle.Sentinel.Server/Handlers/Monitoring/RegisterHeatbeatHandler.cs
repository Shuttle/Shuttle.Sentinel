using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterHeatbeatHandler : IMessageHandler<RegisterHeartbeatCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterHeartbeatCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}