using System;
using System.Collections.Generic;
using System.Linq;

namespace Shuttle.Sentinel.DataAccess.Query
{
    public class LogEntry
    {
        public System.Guid EndpointId { get; set; }
        public string EnvironmentName { get; set; }
        public string MachineName { get; set; }
        public string BaseDirectory { get; set; }
        public string EntryAssemblyQualifiedName { get; set; }
        public System.DateTime DateLogged { get; set; }
        public System.DateTime DateRegistered { get; set; }
        public string Message { get; set; }
        public int? LogLevel { get; set; }
        public string Category { get; set; }
        public int? EventId { get; set; }
        public string Scope { get; set; }

        public class Specification
        {
            private List<int> _logLevels = new List<int>();
            public int MaximumRows { get; private set; } = 100;
            public DateTime? StartDateLogged { get; private set; }
            public DateTime? EndDateLogged { get; private set; }
            public string CategoryMatch { get; private set; }
            public string MessageMatch { get; private set; }
            public string ScopeMatch { get; private set; }
            public string MachineNameMatch { get; private set; }
            public IEnumerable<int> LogLevels => _logLevels.AsReadOnly();

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

            public Specification WithDateLogged(DateTime startDateLogged, DateTime? endDateLogged = null)
            {
                StartDateLogged = startDateLogged;
                EndDateLogged = endDateLogged;

                return this;
            }

            public Specification WithLogLevels(IEnumerable<int> logLevels)
            {
                _logLevels = new List<int>(logLevels ?? Enumerable.Empty<int>());

                return this;
            }

            public Specification MatchingCategory(string categoryMatch)
            {
                CategoryMatch = categoryMatch;

                return this;
            }

            public Specification MatchingMessage(string messageMatch)
            {
                MessageMatch = messageMatch;

                return this;
            }

            public Specification MatchingScope(string scopeMatch)
            {
                ScopeMatch = scopeMatch;

                return this;
            }

            public Specification MatchingMachineName(string machineNameMatch)
            {
                MachineNameMatch = machineNameMatch;

                return this;
            }
        }
    }
}