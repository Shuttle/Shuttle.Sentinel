using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.LogEntry;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.WebApi.Models.v1;

namespace Shuttle.Sentinel.WebApi.Controllers.v1
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
        [HttpPost("search")]
        public IActionResult Search([FromBody] LogEntrySpecificationModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            var specification = new LogEntry.Specification()
                .WithLogLevels(model.LogLevels)
                .MatchingCategory(model.CategoryMatch)
                .MatchingMachineName(model.MachineNameMatch)
                .MatchingMessage(model.MessageMatch)
                .MatchingScope(model.ScopeMatch)
                .WithMaximumRows(model.MaximumRows);

            if (model.StartDateLogged.HasValue)
            {
                specification.WithDateLogged(model.StartDateLogged.Value, model.EndDateLogged);
            }

            using (_databaseContextFactory.Create())
            {
                return Ok(_logEntryQuery.Search(specification));
            }
        }
    }
}