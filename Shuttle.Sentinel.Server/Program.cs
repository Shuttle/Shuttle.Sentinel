using Shuttle.Core.ServiceHost;

namespace Shuttle.Sentinel.Server
{
    public class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}