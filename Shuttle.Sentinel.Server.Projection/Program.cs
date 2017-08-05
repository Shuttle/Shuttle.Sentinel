using Shuttle.Core.ServiceHost;

namespace Shuttle.Sentinel.Server.Projection
{
    public class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}