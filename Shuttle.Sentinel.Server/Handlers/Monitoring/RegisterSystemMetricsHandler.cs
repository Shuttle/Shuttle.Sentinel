using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterSystemMetricsHandler : IMessageHandler<RegisterSystemMetrics>
    {
        public void ProcessMessage(IHandlerContext<RegisterSystemMetrics> context)
        {
            throw new System.NotImplementedException();
        }
    }
}