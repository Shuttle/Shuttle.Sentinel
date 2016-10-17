CREATE TABLE [dbo].[EventStore] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Version]        INT              NOT NULL,
    [TypeId]         UNIQUEIDENTIFIER NOT NULL,
    [Data]           VARBINARY (MAX)  NOT NULL,
    [SequenceNumber] BIGINT           IDENTITY (1, 1) NOT NULL,
    [DateRegistered] DATETIME         CONSTRAINT [DF_EventStore_DateRegistered] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EventStore] PRIMARY KEY CLUSTERED ([Id] ASC, [Version] ASC),
    CONSTRAINT [FK_EventStore_TypeStore] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[TypeStore] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_EventStore]
    ON [dbo].[EventStore]([SequenceNumber] ASC);

