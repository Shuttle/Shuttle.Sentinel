using System.Linq;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.InspectionQueue;
using Shuttle.Sentinel.Queues;

namespace Shuttle.Sentinel.WebApi
{
    public class MessagesController : SentinelApiController
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IInspectionQueue _inspectionQueue;
        private readonly ISerializer _serializer;

        [RequiresPermission(SystemPermissions.View.Users)]
        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = from message in _inspectionQueue.Messages().ToList()
                           select new
                           {
                               MessageId = message.MessageId
                           }
                });
            }
        }

        public MessagesController(IDatabaseContextFactory databaseContextFactory, IInspectionQueue inspectionQueue, ISerializer serializer)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(inspectionQueue, "inspectionQueue");
            Guard.AgainstNull(serializer, "serializer");

            _databaseContextFactory = databaseContextFactory;
            _inspectionQueue = inspectionQueue;
            _serializer = serializer;
        }
    }
}