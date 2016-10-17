CREATE TABLE [dbo].[TypeStore] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [AssemblyQualifiedName] VARCHAR (900)    NOT NULL,
    CONSTRAINT [PK_TypeStore] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_TypeStore]
    ON [dbo].[TypeStore]([AssemblyQualifiedName] ASC);

