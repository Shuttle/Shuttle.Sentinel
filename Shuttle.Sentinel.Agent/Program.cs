using Shuttle.Core.ServiceHost;

namespace Shuttle.Sentinel.Agent
{
    public class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}