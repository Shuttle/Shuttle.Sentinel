CREATE TABLE [dbo].[DataStore] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]             VARCHAR (64)     NOT NULL,
    [ConnectionString] VARCHAR (1024)   NOT NULL,
    [ProviderName]     VARCHAR (512)    NOT NULL,
    CONSTRAINT [PK_DataStore] PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE NONCLUSTERED INDEX [IX_DataStore]
    ON [dbo].[DataStore]([Name] ASC);

