using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;

namespace Shuttle.Sentinel.WebApi
{
	public class ServerController : Controller
	{
		[HttpGet]
		[Route("api/server/configuration")]
		public IActionResult GetServerConfiguration()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;

			return Ok(new
			{
				Version = $"{version.Major}.{version.Minor}.{version.Build}"
			});
		}
	}
}