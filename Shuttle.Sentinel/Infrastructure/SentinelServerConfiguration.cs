namespace Shuttle.Sentinel
{
    public class SentinelServerConfiguration : ISentinelServerConfiguration
    {
        public string NoReplyEMailAddress { get; set; }
        public string NoReplyDisplayName { get; set; }
        public string ActivationUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
    }
}  