using System;

namespace Shuttle.Sentinel.Server;

public class ServerOptions
{
    public string NoReplyEMailAddress { get; set; }
    public string NoReplyDisplayName { get; set; }
    public string ActivationUrl { get; set; }
    public string ResetPasswordUrl { get; set; }
    public TimeSpan HeartbeatIntervalDuration { get; set; }
    public TimeSpan DefaultHeartbeatIntervalDuration { get; set; }
}