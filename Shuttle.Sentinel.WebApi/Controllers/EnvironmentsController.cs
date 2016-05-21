using System.Collections.Generic;
using System.Web.Http;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Sentinel.WebApi
{
    public class EnvironmentsController : ApiController
    {
        private readonly IServiceBus _bus;

        public EnvironmentsController(IServiceBus bus)
        {
            Guard.AgainstNull(bus, "bus");

            _bus = bus;
        }

        public IHttpActionResult Get()
        {
            return Ok(new
            {
	            Data = new List<string> {"Development", "QA", "UAT", "Production"}
            });
        }
    }
}