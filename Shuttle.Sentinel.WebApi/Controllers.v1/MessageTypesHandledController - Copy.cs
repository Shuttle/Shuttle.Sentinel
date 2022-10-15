using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.LogEntry;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class LogEntriesController : Controller
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly ILogEntryQuery _logEntryQuery;

        public LogEntriesController(IDatabaseContextFactory databaseContextFactory, ILogEntryQuery logEntryQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(logEntryQuery, nameof(logEntryQuery));

            _databaseContextFactory = databaseContextFactory;
            _logEntryQuery = logEntryQuery;
        }

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpPost]
        public IActionResult GetSearch(string search = null)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(_logEntryQuery.Search(search ?? string.Empty));
            }
        }
    }
}