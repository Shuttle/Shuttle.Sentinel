using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class MessageTypesHandledController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageTypeHandledQuery _messageTypeHandledQuery;

        public MessageTypesHandledController(IDatabaseContextFactory databaseContextFactory,
            IMessageTypeHandledQuery messageTypeHandledQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageTypeHandledQuery, nameof(messageTypeHandledQuery));

            _databaseContextFactory = databaseContextFactory;
            _messageTypeHandledQuery = messageTypeHandledQuery;
        }

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpGet("{search?}")]
        public IActionResult GetSearch(string search = null)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _messageTypeHandledQuery.Search(search ?? string.Empty)
                });
            }
        }
    }
}