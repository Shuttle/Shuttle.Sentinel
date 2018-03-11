CREATE TABLE [dbo].[DataStore] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Name]             VARCHAR (130)    NOT NULL,
    [ConnectionString] VARCHAR (1000)   NOT NULL,
    [ProviderName]     VARCHAR (130)    NOT NULL,
    CONSTRAINT [PK__DataStor__3214EC070E4B2A4B] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DataStore_Name]
    ON [dbo].[DataStore]([Name] ASC);

