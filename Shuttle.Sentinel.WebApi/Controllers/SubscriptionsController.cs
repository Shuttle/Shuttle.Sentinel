using System;
using System.Linq;
using System.Web.Http;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    public class SubscriptionsController : SentinelApiController
    {
        private readonly IServiceBus _bus;
        private readonly IDataStoreDatabaseContextFactory _databaseContextFactory;
        private readonly ISubscriptionQuery _subscriptionQuery;

        public SubscriptionsController(IServiceBus bus, IDataStoreDatabaseContextFactory databaseContextFactory,
            ISubscriptionQuery subscriptionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(subscriptionQuery, "subscriptionQuery");
            Guard.AgainstNull(bus, "bus");

            _databaseContextFactory = databaseContextFactory;
            _subscriptionQuery = subscriptionQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        [Route("api/subscriptions/{dataStoreId}")]
        public IHttpActionResult Get(Guid dataStoreId)
        {
            using (_databaseContextFactory.Create(dataStoreId))
            {
                return Ok(new
                {
                    Data = _subscriptionQuery.All()
                    .Select(subscription=>
                        {
                            string securedUri;

                            try
                            {
                                securedUri = new Uri(subscription.InboxWorkQueueUri).Secured().ToString();
                            }
                            catch
                            {
                                securedUri = "(invalid uri)";
                            }
                            
                            return new
                            {
                                subscription.MessageType,
                                subscription.InboxWorkQueueUri,
                                SecuredUri = securedUri
                            };
                        })
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        public IHttpActionResult Post([FromBody] SubscriptionModel model)
        {
            Guard.AgainstNull(model, "model");

            _bus.Send(new AddSubscriptionCommand
            {
                DataStoreId = model.DataStoreId,
                MessageType = model.MessageType,
                InboxWorkQueueUri = model.InboxWorkQueueUri
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        [Route("api/subscriptions/remove")]
        public IHttpActionResult RemoveSubscription([FromBody] SubscriptionModel model)
        {
            _bus.Send(new RemoveSubscriptionCommand
            {
                DataStoreId = model.DataStoreId,
                MessageType = model.MessageType,
                InboxWorkQueueUri = model.InboxWorkQueueUri
            });

            return Ok();
        }
    }
}