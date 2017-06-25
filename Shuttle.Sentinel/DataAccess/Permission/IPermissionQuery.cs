using System.Collections.Generic;

namespace Shuttle.Sentinel
{
    public interface IPermissionQuery
    {
        IEnumerable<string> Available();
    }
}