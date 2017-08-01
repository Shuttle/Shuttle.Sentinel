using Shuttle.Core.ServiceHost;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}