using System.Collections.Generic;

namespace Shuttle.Sentinel.WebApi.Models.v1;

public class LogEntrySpecificationModel
{
    public int MaximumRows { get; set; }
    public string MachineNameMatch { get; set; }
    public System.DateTime? StartDateLogged { get; set; }
    public System.DateTime? EndDateLogged { get; set; }
    public string MessageMatch { get; set; }
    public string CategoryMatch { get; set; }
    public string ScopeMatch { get; set; }
    public IEnumerable<int> LogLevels { get; set; }
}