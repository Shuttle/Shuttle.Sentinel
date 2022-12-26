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
            public int MaximumRows { get; private set; } = 30;

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

            public Specification WithMaximumRows(int count)
            {
                MaximumRows = count;

                if (MaximumRows < 30)
                {
                    MaximumRows = 30;
                }

                if (MaximumRows > 10000)
                {
                    MaximumRows = 10000;
                }

                return this;
            }
        }
    }
}