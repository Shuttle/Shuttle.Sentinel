namespace Shuttle.Sentinel.Messages.v1
{
    public class EditDataStoreCommand
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }
}