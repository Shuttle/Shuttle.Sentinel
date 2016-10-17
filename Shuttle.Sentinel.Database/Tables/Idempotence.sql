CREATE TABLE [dbo].[Idempotence] (
    [MessageId]            UNIQUEIDENTIFIER NOT NULL,
    [InboxWorkQueueUri]    VARCHAR (265)    NOT NULL,
    [DateStarted]          DATETIME         CONSTRAINT [DF_Idempotence_DateStarted] DEFAULT (getdate()) NOT NULL,
    [AssignedThreadId]     INT              NULL,
    [DateThreadIdAssigned] DATETIME         NULL,
    [MessageHandled]       INT              CONSTRAINT [DF_Idempotence_MessageHandled] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Idempotence] PRIMARY KEY CLUSTERED ([MessageId] ASC)
);

