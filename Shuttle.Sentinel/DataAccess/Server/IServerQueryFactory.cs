using Shuttle.Core.Data;

namespace Shuttle.Sentinel
{
    public interface IServerQueryFactory
    {
        IQuery FindId(string machineName, string baseDirectory);
    }
}