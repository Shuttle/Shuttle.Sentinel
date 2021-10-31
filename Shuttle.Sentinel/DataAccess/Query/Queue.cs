using System;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class Queue
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public string Processor { get; set; }
        public string Type { get; set; }

        public class Specification
        {
            public Guid? Id { get; private set; }

            public string UriMatch { get; private set; }

            public Specification WithId(Guid id)
            {
                Id = id;

                return this;
            }

            public Specification MatchingUri(string uriMatch)
            {
                UriMatch = uriMatch;

                return this;
            }
        }
    }
}