using System;

namespace Shuttle.Sentinel
{
	public interface ISessionRepository
	{
		void Save(Session session);
		Session Get(Guid token);
		void Remove(Guid token);
	    void Renewed(Session session);
	}
}