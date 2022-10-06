using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess.Tag
{
    public class TagQueryFactory : ITagQueryFactory
    {
        public IQuery Register(Guid ownerId, string tag)
        {
            return RawQuery.Create(@"
if not exists
(
    select
        null
    from
        [Tag]
    where
        [OwnerId] = @OwnerId
    and
        [Tag] = @Tag
)
    insert into Tag
    (
	    OwnerId,
	    Tag
    )
    values
    (
	    @OwnerId,
	    @Tag
    );
")
                .AddParameterValue(Columns.OwnerId, ownerId)
                .AddParameterValue(Columns.Tag, tag);
        }

        public IQuery Remove(Guid ownerId, string tag)
        {
            return RawQuery.Create(@"
delete
from
    Tag
where
    [OwnerId] = @OwnerId
and
    [Tag] = @Tag
")
                .AddParameterValue(Columns.OwnerId, ownerId)
                .AddParameterValue(Columns.Tag, tag);
        }

        public IQuery Find(Guid ownerId)
        {
            return RawQuery.Create(@"
select
    Tag
from
    Tag
where
    [OwnerId] = @OwnerId
")
                .AddParameterValue(Columns.OwnerId, ownerId);
        }
    }
}