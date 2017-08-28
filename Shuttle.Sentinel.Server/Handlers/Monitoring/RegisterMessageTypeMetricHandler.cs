using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMessageTypeMetricHandler : IMessageHandler<RegisterMessageTypeMetricCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMessageTypeMetricCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}