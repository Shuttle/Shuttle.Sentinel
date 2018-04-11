using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageHeaderQueryFactory
    {
        IQuery Save(string key, string value);
        IQuery Remove(Guid id);
        IQuery All();
        IQuery Search(string match);
    }
}