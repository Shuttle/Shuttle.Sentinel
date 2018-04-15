using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Module
{
    public interface IEndpointAggregator
    {
        void Reset();
        RegisterEndpointCommand GetRegisterEndpointCommand();
    }
}