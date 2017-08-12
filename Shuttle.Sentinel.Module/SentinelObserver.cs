using System;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.Module
{
    public class SentinelObserver :
        IPipelineObserver<OnHandleMessage>,
        IPipelineObserver<OnAfterHandleMessage>
    {
        public void Execute(OnAfterHandleMessage pipelineEvent)
        {
            throw new NotImplementedException();
        }

        public void Execute(OnHandleMessage pipelineEvent)
        {
            throw new NotImplementedException();
        }
    }
}