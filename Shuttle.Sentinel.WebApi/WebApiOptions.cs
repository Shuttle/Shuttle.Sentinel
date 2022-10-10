using System;

namespace Shuttle.Sentinel.WebApi;

public class WebApiOptions
{
    public const string SectionName = "Shuttle:Sentinel:WebApi";

    public string SiteUrl { get; set; }
    public Type SerializerType { get; set; }
    public TimeSpan HeartbeatRecoveryDuration { get; set; }
}