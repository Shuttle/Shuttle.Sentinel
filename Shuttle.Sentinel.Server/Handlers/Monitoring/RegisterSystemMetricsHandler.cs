using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterSystemMetricsHandler : IMessageHandler<RegisterSystemMetricsCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterSystemMetricsCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}