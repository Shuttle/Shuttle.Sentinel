CREATE TABLE [dbo].[Endpoint] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Endpoint_Id] DEFAULT newid(),
    [MachineName]                VARCHAR (130)    NOT NULL,
    [BaseDirectory]              VARCHAR (260)    NOT NULL,
    [EnvironmentName]              VARCHAR (65)    NOT NULL,
    [InboxWorkQueueUri]          VARCHAR (130)    NULL,
    [EntryAssemblyQualifiedName] VARCHAR (500)    NULL,
	[IPv4Address]				VARCHAR(15) NULL, 
    [InboxDeferredQueueUri]		VARCHAR(130) NULL, 
    [InboxErrorQueueUri]		VARCHAR(130) NOT NULL, 
    [ControlInboxWorkQueueUri]   VARCHAR (130)    NULL,
    [ControlInboxErrorQueueUri]   VARCHAR (130)    NULL,
    [OutboxWorkQueueUri]   VARCHAR (130)    NULL,
    [OutboxErrorQueueUri]   VARCHAR (130)    NULL,
    [TransientInstance] BIT NULL, 
    [HeartbeatDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [HeartbeatIntervalDuration] VARCHAR(25) NOT NULL DEFAULT '00:00:30', 
    [DateStarted] DATETIME2 NULL,
    [DateStopped] DATETIME2 NULL, 
    CONSTRAINT [PK_Endpoint] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Endpoint]
    ON [dbo].[Endpoint] ([MachineName] ASC, [BaseDirectory] ASC);

