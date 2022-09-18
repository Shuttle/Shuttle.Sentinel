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
    public class MessageTypeMetricsController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageTypeMetricQuery _messageTypeMetricQuery;

        public MessageTypeMetricsController(IDatabaseContextFactory databaseContextFactory,
            IMessageTypeMetricQuery messageTypeMetricQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageTypeMetricQuery, nameof(messageTypeMetricQuery));

            _databaseContextFactory = databaseContextFactory;
            _messageTypeMetricQuery = messageTypeMetricQuery;
        }

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpPost("search")]
        public IActionResult Search([FromBody] MessageTypeMetricSearchModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _messageTypeMetricQuery.Search(model.From, model.Match ?? string.Empty)
                });
            }
        }
    }
}