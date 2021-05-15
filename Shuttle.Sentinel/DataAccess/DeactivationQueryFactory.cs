using System;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel.DataAccess
{
    public class DeactivationQueryFactory : IDeactivationQueryFactory
    {
        public string Deactivate(string table, Guid id)
        {
            Guard.AgainstNullOrEmptyString(table, nameof(table));

            return $@"
update
    {table}
set
    EffectiveToDate = @EffectiveFromDate
where
    Id = (select top 1 Id from {table} where SentinelId = @SentinelId order by EffectiveFromDate desc)
";
        }
    }
}