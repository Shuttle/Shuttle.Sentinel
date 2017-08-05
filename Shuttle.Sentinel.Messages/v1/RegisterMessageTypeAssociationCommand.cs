namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypeAssociationCommand
    {
        public MessageTypeAssociation Association { get; set; }

        public class MessageTypeAssociation
        {
            public string MessageTypeHandled { get; set; }
            public string MessageTypeDispatched { get; set; }
        }
    }
}