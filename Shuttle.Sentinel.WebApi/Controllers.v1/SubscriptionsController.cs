using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;
using Shuttle.Sentinel.WebApi.Models.v1;

namespace Shuttle.Sentinel.WebApi.Controllers.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class SubscriptionsController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ISubscriptionQuery _subscriptionQuery;

        public SubscriptionsController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            ISubscriptionQuery subscriptionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(subscriptionQuery, nameof(subscriptionQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _subscriptionQuery = subscriptionQuery;
            _bus = bus;
        }

        [HttpGet]
        [RequiresPermission(Permissions.Manage.Subscriptions)]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(_subscriptionQuery.All());
            }
        }

        [RequiresPermission(Permissions.Manage.Subscriptions)]
        [HttpPost]
        public IActionResult Post([FromBody] SubscriptionModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new AddSubscription
            {
                MessageType = model.MessageType,
                InboxWorkQueueUri = model.InboxWorkQueueUri
            });

            return Ok();
        }

        [RequiresPermission(Permissions.Manage.Subscriptions)]
        [HttpPost("remove")]
        public IActionResult RemoveSubscription([FromBody] SubscriptionModel model)
        {
            _bus.Send(new RemoveSubscription
            {
                MessageType = model.MessageType,
                InboxWorkQueueUri = model.InboxWorkQueueUri
            });

            return Ok();
        }
    }
}