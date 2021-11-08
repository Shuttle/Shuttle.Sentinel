using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.Scheduling;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;
using Schedule = Shuttle.Esb.Scheduling.Query.Schedule;

namespace Shuttle.Sentinel.WebApi
{
    [Route("[controller]")]
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

        [RequiresPermission(Permissions.Manage.Schedules)]
        [HttpGet("{dataStoreId}/{id}")]
        public IActionResult Get(Guid dataStoreId, Guid id)
        {
            try
            {
                using (_databaseContextFactory.Create(dataStoreId))
                {
                    return Ok(_scheduleQuery.Search(new Schedule.Specification().WithId(id))
                        .Select(schedule => GetSchedule(dataStoreId, schedule)).FirstOrDefault()
                        .GuardAgainstRecordNotFound(id)
                    );
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static object GetSchedule(Guid dataStoreId, Schedule schedule)
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
        }

        [RequiresPermission(Permissions.Manage.Schedules)]
        [HttpGet("search/{dataStoreId}/{match?}")]
        public IActionResult Search(Guid dataStoreId, string match = null)
        {
            try
            {
                using (_databaseContextFactory.Create(dataStoreId))
                {
                    return Ok(_scheduleQuery.Search(new Schedule.Specification().MatchingFuzzy(match))
                        .Select(schedule => GetSchedule(dataStoreId, schedule))
                    );
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RequiresPermission(Permissions.Manage.Schedules)]
        [HttpPost]
        public IActionResult Post([FromBody] ScheduleModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            var message = new RegisterScheduleCommand
            {
                DataStoreId = model.DataStoreId,
                Id = model.Id ?? Guid.Empty,
                Name = model.Name,
                InboxWorkQueueUri = model.InboxWorkQueueUri,
                CronExpression = model.CronExpression,
                NextNotification = model.NextNotification
            };

            try
            {
                message.ApplyInvariants();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _bus.Send(message);

            return Ok();
        }

        [RequiresPermission(Permissions.Manage.Schedules)]
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