namespace Shuttle.Sentinel
{
	public interface IHashingService
	{
		byte[] Sha256(string password);
	}
}