CREATE TABLE [dbo].[Endpoint] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Endpoint_Id] DEFAULT newid(),
    [EndpointName]               VARCHAR (250)    NOT NULL,
    [MachineName]                VARCHAR (130)    NOT NULL,
    [BaseDirectory]              VARCHAR (260)    NOT NULL,
    [EntryAssemblyQualifiedName] VARCHAR (500)    NOT NULL,
    [InboxWorkQueueUri]          VARCHAR (130)    NOT NULL,
    [ControlInboxWorkQueueUri]   VARCHAR (130)    NOT NULL,
    CONSTRAINT [PK_Endpoint] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Endpoint]
    ON [dbo].[Endpoint]([EndpointName] ASC, [MachineName] ASC, [BaseDirectory] ASC);

