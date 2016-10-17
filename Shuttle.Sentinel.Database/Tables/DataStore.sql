CREATE TABLE [dbo].[DataStore] (
    [Name]             VARCHAR (64)   NOT NULL,
    [ConnectionString] VARCHAR (1024) NOT NULL,
    [ProviderName]     VARCHAR (512)  NOT NULL,
    CONSTRAINT [PK_DataStore] PRIMARY KEY CLUSTERED ([Name] ASC)
);

