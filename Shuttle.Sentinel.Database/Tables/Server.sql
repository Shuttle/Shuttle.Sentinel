CREATE TABLE [dbo].[Server] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Server_Id] DEFAULT newid(),
    [MachineName]                VARCHAR (130)    NOT NULL,
    [BaseDirectory]              VARCHAR (260)    NOT NULL,
    [IPv4Address]                VARCHAR (20)     NOT NULL,
    [InboxWorkQueueUri]          VARCHAR (130)    NOT NULL,
    [ControlInboxWorkQueueUri]   VARCHAR (130)    NULL,
    CONSTRAINT [PK_Server] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Server]
    ON [dbo].[Server]([MachineName] ASC, [BaseDirectory] ASC);


GO

CREATE UNIQUE INDEX [IX_Server_ControlInboxWorkQueueUri] ON [dbo].[Server] ([ControlInboxWorkQueueUri])
