namespace Shuttle.Sentinel.WebApi
{
    public class AddDataStoreModel
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; } 
    }
}