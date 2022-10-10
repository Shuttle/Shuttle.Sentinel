using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.Scheduling;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class ScheduleHandler :
        IMessageHandler<RegisterSchedule>,
        IMessageHandler<RemoveSchedule>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleHandler(IDatabaseContextFactory databaseContextFactory,
            IScheduleRepository scheduleRepository)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(scheduleRepository, nameof(scheduleRepository));

            _databaseContextFactory = databaseContextFactory;
            _scheduleRepository = scheduleRepository;
        }

        public void ProcessMessage(IHandlerContext<RemoveSchedule> context)
        {
            Guard.AgainstNull(context, nameof(context));

            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                _scheduleRepository.Remove(message.Id);
            }
        }

        public void ProcessMessage(IHandlerContext<RegisterSchedule> context)
        {
            Guard.AgainstNull(context, "context");

            var message = context.Message;

            try
            {
                var uri = new Uri(message.InboxWorkQueueUri);
            }
            catch
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                if (_scheduleRepository.Contains(message.Name, message.InboxWorkQueueUri, message.CronExpression))
                {
                    return;
                }

                _scheduleRepository.Save(new Schedule(
                    message.Id.Equals(Guid.Empty) ? Guid.NewGuid() : message.Id,
                    message.Name,
                    message.InboxWorkQueueUri,
                    message.CronExpression,
                    message.NextNotification
                ));
            }
        }
    }
}