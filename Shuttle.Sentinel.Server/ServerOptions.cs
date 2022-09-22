using System;

namespace Shuttle.Sentinel.Server;

public class ServerOptions
{
    public const string SectionName = "Shuttle:Sentinel:Server";

    public string NoReplyEMailAddress { get; set; }
    public string NoReplyDisplayName { get; set; }
    public string ActivationUrl { get; set; }
    public string ResetPasswordUrl { get; set; }
    public TimeSpan HeartbeatIntervalDuration { get; set; } = TimeSpan.FromSeconds(30);
}