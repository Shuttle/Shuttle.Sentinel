using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.WebApi.Controllers.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class MessageTypesDispatchedController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageTypeDispatchedQuery _messageTypeDispatchedQuery;

        public MessageTypesDispatchedController(IDatabaseContextFactory databaseContextFactory,
            IMessageTypeDispatchedQuery messageTypeDispatchedQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageTypeDispatchedQuery, nameof(messageTypeDispatchedQuery));

            _databaseContextFactory = databaseContextFactory;
            _messageTypeDispatchedQuery = messageTypeDispatchedQuery;
        }

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpGet("{search?}")]
        public IActionResult GetSearch(string search = null)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(_messageTypeDispatchedQuery.Search(search ?? string.Empty));
            }
        }
    }
}