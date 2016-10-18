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
else
    update DataStore set
        ConnectionString = @ConnectionString,
        ProviderName = @ProviderName
    where
        Name = @Name
")
                .AddParameterValue(DataStoreColumns.Name, dataStore.Name)
                .AddParameterValue(DataStoreColumns.ConnectionString, dataStore.ConnectionString)
                .AddParameterValue(DataStoreColumns.ProviderName, dataStore.ProviderName);
        }

        public IQuery Remove(string name)
        {
            return RawQuery.Create(
                @"delete from DataStore where Name = @Name")
                .AddParameterValue(DataStoreColumns.Name, name);
        }

        public IQuery All()
        {
            return RawQuery.Create(@"select Name, ConnectionString, ProviderName from DataStore order by Name");
        }
    }
}