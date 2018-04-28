using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.WebApi
{
    [Route("api/[controller]")]
    public class MessageTypesHandledController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageTypeHandledQuery _messageTypesHandledQuery;

        public MessageTypesHandledController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IMessageTypeHandledQuery messageTypesHandledQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageTypesHandledQuery, nameof(messageTypesHandledQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _messageTypesHandledQuery = messageTypesHandledQuery;
        }

        [RequiresPermission(SystemPermissions.Manage.Endpoints)]
        [HttpGet("{search?}")]
        public IActionResult GetSearch(string search = null)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _messageTypesHandledQuery.Search(search ?? string.Empty)
                });
            }
        }
    }
}