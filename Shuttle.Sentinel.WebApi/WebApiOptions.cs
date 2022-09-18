using System;

namespace Shuttle.Sentinel.WebApi;

public class WebApiOptions
{
    public string SiteUrl { get; set; }
    public Type SerializerType { get; set; }
    public TimeSpan HeartbeatRecoveryDuration { get; set; }
}