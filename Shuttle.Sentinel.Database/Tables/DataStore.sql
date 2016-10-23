CREATE TABLE [dbo].[DataStore] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(), 
    [Name]             VARCHAR (64)   NOT NULL,
    [ConnectionString] VARCHAR (1024) NOT NULL,
    [ProviderName]     VARCHAR (512)  NOT NULL, 
    CONSTRAINT [PK_DataStore] PRIMARY KEY ([Id])
);


GO

CREATE INDEX [IX_DataStore] ON [dbo].[DataStore] ([Name])
