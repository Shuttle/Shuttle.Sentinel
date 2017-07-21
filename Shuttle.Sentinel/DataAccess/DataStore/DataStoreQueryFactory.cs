using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.Query;

namespace Shuttle.Sentinel
{
    public class DataStoreQueryFactory : IDataStoreQueryFactory
    {
        public IQuery Add(DataStore dataStore)
        {
            return RawQuery.Create(@"
if not exists (select null from DataStore where Name = @Name)
    insert into DataStore 
    (
        Name,
        ConnectionString,
        ProviderName
    ) 
    values 
    (
        @Name,
        @ConnectionString,
        @ProviderName
    )
")
                .AddParameterValue(DataStoreColumns.Name, dataStore.Name)
                .AddParameterValue(DataStoreColumns.ConnectionString, dataStore.ConnectionString)
                .AddParameterValue(DataStoreColumns.ProviderName, dataStore.ProviderName);
        }

        public IQuery Remove(Guid id)
        {
            return RawQuery.Create(@"
delete 
from 
    DataStore 
where 
    Id = @Id
")
                .AddParameterValue(Columns.Id, id);
        }

        public IQuery All()
        {
            return RawQuery.Create(@"select Id, Name, ConnectionString, ProviderName from DataStore order by Name");
        }

        public IQuery Get(Guid id)
        {
            return RawQuery.Create(@"
select 
    Id, 
    Name, 
    ConnectionString, 
    ProviderName 
from 
    DataStore 
where
    Id = @Id
")
                .AddParameterValue(Columns.Id, id);
        }
    }
}