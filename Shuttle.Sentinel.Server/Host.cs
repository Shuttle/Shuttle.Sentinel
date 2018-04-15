using System;
using System.Net;
using System.Net.Sockets;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;
using Shuttle.Recall;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private WindsorContainer _container;

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _container = new WindsorContainer();

            var container = new WindsorComponentContainer(_container);

            container.RegisterSuffixed("Shuttle.Sentinel");

            EventStore.Register(container);
            ServiceBus.Register(container);

            container.Resolve<IDataStoreDatabaseContextFactory>().ConfigureWith("Sentinel");
            container.Resolve<IDatabaseContextFactory>().ConfigureWith("Sentinel");

            _bus = ServiceBus.Create(container).Start();

            var ipv4Address = "0.0.0.0";

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                ipv4Address = ip.ToString();
            }

            var serviceBusConfiguration = container.Resolve<IServiceBusConfiguration>();

            _bus.Send(new RegisterServerCommand
            {
                MachineName = Environment.MachineName,
                IPv4Address = ipv4Address,
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory,
                InboxWorkQueueUri = serviceBusConfiguration.Inbox.WorkQueueUri,
                InboxDeferredQueueUri = serviceBusConfiguration.Inbox.HasDeferredQueue
                    ? serviceBusConfiguration.Inbox.DeferredQueueUri
                    : string.Empty,
                InboxErrorQueueUri = serviceBusConfiguration.Inbox.ErrorQueueUri,
                OutboxWorkQueueUri = serviceBusConfiguration.HasOutbox
                    ? serviceBusConfiguration.Outbox.WorkQueueUri
                    : string.Empty,
                OutboxErrorQueueUri = serviceBusConfiguration.HasOutbox
                    ? serviceBusConfiguration.Outbox.ErrorQueueUri
                    : string.Empty,
                ControlInboxWorkQueueUri = serviceBusConfiguration.HasControlInbox
                    ? serviceBusConfiguration.ControlInbox.WorkQueueUri
                    : string.Empty,
                ControlInboxErrorQueueUri = serviceBusConfiguration.HasControlInbox
                    ? serviceBusConfiguration.ControlInbox.ErrorQueueUri
                    : string.Empty
            }, c => c.Local());
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}