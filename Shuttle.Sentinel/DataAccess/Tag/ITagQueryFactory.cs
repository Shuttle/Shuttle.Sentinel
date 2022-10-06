using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess.Tag
{
    public interface ITagQueryFactory
    {
        IQuery Register(Guid ownerId, string tag);
        IQuery Remove(Guid ownerId, string tag);
        IQuery Find(Guid ownerId);
    }
}