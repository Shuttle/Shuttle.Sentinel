namespace Shuttle.Sentinel.Messages.v1
{
    public class AddMessageHeaderCommand
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}