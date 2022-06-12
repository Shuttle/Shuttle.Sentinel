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
    public class MessageTypeAssociationsController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageTypeAssociationQuery _messageTypeAssociationQuery;

        public MessageTypeAssociationsController(IDatabaseContextFactory databaseContextFactory,
            IMessageTypeAssociationQuery messageTypeAssociationQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageTypeAssociationQuery, nameof(messageTypeAssociationQuery));

            _databaseContextFactory = databaseContextFactory;
            _messageTypeAssociationQuery = messageTypeAssociationQuery;
        }

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpGet("{search?}")]
        public IActionResult GetSearch(string search = null)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _messageTypeAssociationQuery.Search(search ?? string.Empty)
                });
            }
        }
    }
}