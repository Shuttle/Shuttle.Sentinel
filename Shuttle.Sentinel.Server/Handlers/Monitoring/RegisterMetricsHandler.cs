using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class RegisterMetricsHandler : IMessageHandler<RegisterMetricsCommand>
    {
        public void ProcessMessage(IHandlerContext<RegisterMetricsCommand> context)
        {
            var message = context.Message;

            foreach (var metric in message.MessageMetrics)
            {
                
            }

            foreach (var association in message.MessageTypeAssociations)
            {
                
            }

            foreach (var dispatched in message.MessageTypesDispatched)
            {
                
            }

            foreach (var messageType in message.MessageTypesHandled)
            {
                
            }

            foreach (var systemMetric in message.SystemMetrics)
            {
                
            }
        }
    }
}