using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    [Route("api/[controller]")]
    public class SubscriptionsController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDataStoreDatabaseContextFactory _databaseContextFactory;
        private readonly ISubscriptionQuery _subscriptionQuery;

        public SubscriptionsController(IServiceBus bus, IDataStoreDatabaseContextFactory databaseContextFactory,
            ISubscriptionQuery subscriptionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(subscriptionQuery, nameof(subscriptionQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _subscriptionQuery = subscriptionQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        [HttpGet("{dataStoreId}")]
        public IActionResult Get(Guid dataStoreId)
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
                                DataStoreId = dataStoreId,
                                subscription.MessageType,
                                subscription.InboxWorkQueueUri,
                                SecuredUri = securedUri
                            };
                        })
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        [HttpPost]
        public IActionResult Post([FromBody] SubscriptionModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new AddSubscriptionCommand
            {
                DataStoreId = model.DataStoreId,
                MessageType = model.MessageType,
                InboxWorkQueueUri = model.InboxWorkQueueUri
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Subscriptions)]
        [HttpPost("remove")]
        public IActionResult RemoveSubscription([FromBody] SubscriptionModel model)
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