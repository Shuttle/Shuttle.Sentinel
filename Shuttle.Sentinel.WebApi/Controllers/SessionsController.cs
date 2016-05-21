using System.Web.Http;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
	public class SessionsController : ApiController
	{
		private readonly ISessionService _sessionService;

		public SessionsController(ISessionService sessionService)
		{
			Guard.AgainstNull(sessionService, "sessionService");

			_sessionService = sessionService;
		}

		public IHttpActionResult Post([FromBody] RegisterSessionModel model)
		{
			Guard.AgainstNull(model, "model");

			var registerSessionResult = _sessionService.Register(model.EMail, model.Password);

			return registerSessionResult.Ok
				? (IHttpActionResult) Ok(new
				{
					Registered = true,
					Token = registerSessionResult.Token.ToString("n"),
					registerSessionResult.Permissions
				})
				: Ok(new
				{
					Registered = false
				});
		}
	}
}