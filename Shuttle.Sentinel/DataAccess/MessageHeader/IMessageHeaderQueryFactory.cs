using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageHeaderQueryFactory
    {
        IQuery Save(Guid id, string key, string value);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Search(string match);
    }
}