namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypeAssociationCommand
    {
        public string MessageTypeHandled { get; set; }
        public string MessageTypeDispatched { get; set; }
    }
}