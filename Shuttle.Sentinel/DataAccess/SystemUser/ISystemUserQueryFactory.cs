using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DomainEvents.User.v1;

namespace Shuttle.Sentinel
{
	public interface ISystemUserQueryFactory
	{
		IQuery Register(Guid id, Registered domainEvent);
		IQuery Count();
	}
}