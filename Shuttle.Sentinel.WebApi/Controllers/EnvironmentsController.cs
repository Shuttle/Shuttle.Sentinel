using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Esb;

namespace Shuttle.Sentinel.WebApi
{
    public class EnvironmentsController : Controller
    {
        private readonly IServiceBus _bus;

        public EnvironmentsController(IServiceBus bus)
        {
            Guard.AgainstNull(bus, nameof(bus));

            _bus = bus;
        }

        public IActionResult Get()
        {
            return Ok(new
            {
	            Data = new List<string> {"Development", "QA", "UAT", "Production"}
            });
        }
    }
}