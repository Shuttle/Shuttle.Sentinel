CREATE TABLE [dbo].[DataStore]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(130) NOT NULL, 
    [ConnectionString] VARCHAR(260) NOT NULL, 
    [ProviderName] VARCHAR(130) NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_DataStore_Name] ON [dbo].[DataStore] ([Name])
