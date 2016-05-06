using System;
using Shuttle.Esb;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
	public class UserRegiseredHandler : IMessageHandler<UserRegisteredEvent>
	{
		public void ProcessMessage(IHandlerContext<UserRegisteredEvent> context)
		{
			if (!context.Message.RegisteredBy.Equals("system", StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			context.Send(new ActivateUserCommand { Id = context.Message.Id }, c => c.Local());
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}