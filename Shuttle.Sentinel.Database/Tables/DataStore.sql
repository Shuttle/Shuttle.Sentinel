CREATE TABLE [dbo].[DataStore]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(64) NOT NULL, 
    [ConnectionString] VARCHAR(1024) NOT NULL, 
    [ProviderName] VARCHAR(128) NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_DataStore_Name] ON [dbo].[DataStore] ([Name])
