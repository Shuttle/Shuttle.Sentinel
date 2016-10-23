using System;

namespace Shuttle.Sentinel.WebApi
{
    public class EditDataStoreModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; } 
    }
}