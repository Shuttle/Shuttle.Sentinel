CREATE TABLE [dbo].[IdempotenceHistory] (
    [MessageId]         UNIQUEIDENTIFIER NOT NULL,
    [InboxWorkQueueUri] VARCHAR (265)    NOT NULL,
    [DateStarted]       DATETIME         NOT NULL,
    [DateCompleted]     DATETIME         CONSTRAINT [DF_IdempotenceHistory_DateStarted] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_IdempotenceHistory] PRIMARY KEY CLUSTERED ([MessageId] ASC)
);

