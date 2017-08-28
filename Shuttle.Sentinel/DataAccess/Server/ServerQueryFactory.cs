using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public class ServerQueryFactory : IServerQueryFactory
    {
        public IQuery FindId(string machineName, string baseDirectory)
        {
            return RawQuery.Create(@"
select
    Id
from
    Server
where
    MachineName = @MachineName
and
    BaseDirectory = @BaseDirectory
")
                .AddParameterValue(ServerColumns.MachineName, machineName)
                .AddParameterValue(ServerColumns.BaseDirectory, baseDirectory);
        }
    }
}