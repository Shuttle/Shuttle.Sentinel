using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class DataStore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        public class Specification
        {
            public Guid? Id { get; private set; }

            public Specification WithId(Guid id)
            {
                Id = id;

                return this;
            }
        }
    }
}