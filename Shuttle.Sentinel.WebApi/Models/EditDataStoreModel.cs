namespace Shuttle.Sentinel.WebApi
{
    public class EditDataStoreModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; } 
    }
}