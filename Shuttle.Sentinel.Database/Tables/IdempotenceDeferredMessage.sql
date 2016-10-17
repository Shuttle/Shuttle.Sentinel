CREATE TABLE [dbo].[IdempotenceDeferredMessage] (
    [MessageId]         UNIQUEIDENTIFIER NOT NULL,
    [MessageIdReceived] UNIQUEIDENTIFIER NOT NULL,
    [MessageBody]       IMAGE            NOT NULL,
    CONSTRAINT [PK_IdempotenceDeferredMessage] PRIMARY KEY NONCLUSTERED ([MessageId] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_IdempotenceDeferredMessage]
    ON [dbo].[IdempotenceDeferredMessage]([MessageIdReceived] ASC);

