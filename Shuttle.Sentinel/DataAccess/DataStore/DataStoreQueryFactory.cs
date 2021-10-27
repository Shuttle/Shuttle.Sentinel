using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public class DataStoreQueryFactory : IDataStoreQueryFactory
    {
        public IQuery Add(DataStore dataStore)
        {
            return RawQuery.Create(@"
if not exists (select null from DataStore where Name = @Name)
    insert into DataStore 
    (
        Id,
        Name,
        ConnectionString,
        ProviderName
    ) 
    values 
    (
        @Id,
        @Name,
        @ConnectionString,
        @ProviderName
    )
")
                .AddParameterValue(Columns.Id, dataStore.Id)
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

        public IQuery Search(DataStore.Specification specification)
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
    (
        @Id is null
        or
        Id = @Id
    )
order by 
    Name")
                .AddParameterValue(Columns.Id, specification.Id);
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