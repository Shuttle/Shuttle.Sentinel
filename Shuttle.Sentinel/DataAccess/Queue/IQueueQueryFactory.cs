using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IQueueQueryFactory
    {
        IQuery Save(string uri, string processor, string type);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Search(Query.Queue.Specification specification);
    }
}