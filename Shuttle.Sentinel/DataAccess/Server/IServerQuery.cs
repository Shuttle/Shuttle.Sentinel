using System;

namespace Shuttle.Sentinel
{
    public interface IServerQuery
    {
        Guid? FindId(string machineName, string baseDirectory);
    }
}