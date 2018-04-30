using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Esb;
using Shuttle.Esb.Scheduling;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDataStoreDatabaseContextFactory _databaseContextFactory;
        private readonly IScheduleQuery _scheduleQuery;

        public SchedulesController(IServiceBus bus, IDataStoreDatabaseContextFactory databaseContextFactory,
            IScheduleQuery scheduleQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(scheduleQuery, nameof(scheduleQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _scheduleQuery = scheduleQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Schedules)]
        [HttpGet("{dataStoreId}/{search?}")]
        public IActionResult Get(Guid dataStoreId, string match = null)
        {
            using (_databaseContextFactory.Create(dataStoreId))
            {
                return Ok(new
                {
                    Data = _scheduleQuery.Search(match ?? string.Empty)
                    .Select(schedule=>
                        {
                            string securedUri;

                            try
                            {
                                securedUri = new Uri(schedule.InboxWorkQueueUri).Secured().ToString();
                            }
                            catch
                            {
                                securedUri = "(invalid uri)";
                            }
                            
                            return new
                            {
                                DataStoreId = dataStoreId,
                                schedule.Id,
                                schedule.Name,
                                schedule.InboxWorkQueueUri,
                                SecuredUri = securedUri,
                                schedule.CronExpression,
                                schedule.NextNotification
                            };
                        })
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Schedules)]
        [HttpPost]
        public IActionResult Post([FromBody] ScheduleModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new SaveScheduleCommand
            {
                DataStoreId = model.DataStoreId,
                Id = model.Id,
                Name=model.Name,
                InboxWorkQueueUri = model.InboxWorkQueueUri,
                CronExpression = model.CronExpression,
                NextNotification = model.NextNotification
            });

            return Ok();
        }

        [RequiresPermission(SystemPermissions.Manage.Schedules)]
        [HttpDelete("{dataStoreId}/{id}")]
        public IActionResult RemoveSchedule(Guid dataStoreId, Guid id)
        {
            _bus.Send(new RemoveScheduleCommand
            {
                DataStoreId = dataStoreId,
                Id = id
            });

            return Ok();
        }
    }
}