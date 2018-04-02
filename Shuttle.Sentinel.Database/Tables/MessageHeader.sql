CREATE TABLE [dbo].[MessageHeader] (
    [Id]    UNIQUEIDENTIFIER NOT NULL,
    [Key]   VARCHAR (130)    NOT NULL,
    [Value] VARCHAR (4096)   NOT NULL,
    CONSTRAINT [PK_MessageHeader] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);



GO


