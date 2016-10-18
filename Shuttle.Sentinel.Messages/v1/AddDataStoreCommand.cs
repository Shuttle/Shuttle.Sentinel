namespace Shuttle.Sentinel.Messages.v1
{
    public class AddDataStoreCommand
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }
}