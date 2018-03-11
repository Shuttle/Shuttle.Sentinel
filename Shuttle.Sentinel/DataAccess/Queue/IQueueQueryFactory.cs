using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IQueueQueryFactory
    {
        IQuery Add(string uri, string displayUri);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Search(string match);
    }
}