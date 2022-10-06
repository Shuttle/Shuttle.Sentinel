using System;
using System.Collections;
using System.Collections.Generic;

namespace Shuttle.Sentinel.DataAccess.Tag
{
    public interface ITagQuery
    {
        void Register(Guid ownerId, string tag);
        void Remove(Guid ownerId, string tag);
        IEnumerable<string> Find(Guid ownerId);
    }
}