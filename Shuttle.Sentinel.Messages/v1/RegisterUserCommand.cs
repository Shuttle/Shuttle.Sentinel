namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterUserCommand
    {
		public string EMail { get; set; }
	    public byte[] PasswordHash { get; set; }
	    public string RegisteredBy { get; set; }
    }
}
