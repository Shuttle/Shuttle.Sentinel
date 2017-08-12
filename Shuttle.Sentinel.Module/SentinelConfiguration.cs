using System;
using System.Net;
using System.Net.Sockets;

namespace Shuttle.Sentinel.Module
{
    public class SentinelConfiguration : ISentinelConfiguration
    {
        public SentinelConfiguration()
        {
            IPv4Address = "0.0.0.0";

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                IPv4Address = ip.ToString();
            }

            MachineName = Environment.MachineName;
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public string InboxWorkQueueUri { get; set; }
        public string EndpointName { get; set; }
        public string MachineName { get; }
        public string BaseDirectory { get; }
        public string IPv4Address { get; }
        public int HeartbeatIntervalSeconds { get; set; }
    }
}