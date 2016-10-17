CREATE TABLE [dbo].[SnapshotStore] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Version]        INT              NOT NULL,
    [TypeId]         UNIQUEIDENTIFIER NOT NULL,
    [Data]           VARBINARY (MAX)  NOT NULL,
    [DateRegistered] DATETIME         CONSTRAINT [DF_SnapshotStore_DateRegistered] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SnapshotStore] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SnapshotStore_TypeStore] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[TypeStore] ([Id])
);

