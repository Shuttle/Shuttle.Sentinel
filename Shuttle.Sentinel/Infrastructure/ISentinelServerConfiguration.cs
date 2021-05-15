namespace Shuttle.Sentinel
{
    public interface ISentinelServerConfiguration
    {
        string NoReplyEMailAddress { get; }
        string NoReplyDisplayName { get; }
        string ActivationUrl { get; }
        string ResetPasswordUrl { get; }
    }
}                           