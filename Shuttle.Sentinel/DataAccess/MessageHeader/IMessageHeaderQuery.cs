using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageHeaderQuery
    {
        void Save(string key, string value);
        void Remove(Guid id);
        IEnumerable<MessageHeader> All();
        IEnumerable<MessageHeader> Search(string match);
    }
}