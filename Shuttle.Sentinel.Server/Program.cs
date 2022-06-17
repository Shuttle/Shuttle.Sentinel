using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using Shuttle.Core.WorkerService;

namespace Shuttle.Sentinel.Server
{
    public class Program
    {
        private static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ServiceHost.Run<Host>();
        }
    }

    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}