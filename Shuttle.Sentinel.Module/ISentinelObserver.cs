using Shuttle.Core.Pipelines;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public interface ISentinelObserver :
        IPipelineObserver<OnBeforeHandleMessage>,
        IPipelineObserver<OnAfterHandleMessage>,
        IPipelineObserver<OnPipelineException>,
        IPipelineObserver<OnAfterDispatchTransportMessage>
    {
    }
}