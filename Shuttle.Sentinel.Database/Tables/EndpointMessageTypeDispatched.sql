CREATE TABLE [dbo].[EndpointMessageTypeDispatched] (
    [EndpointId]  UNIQUEIDENTIFIER NOT NULL,
    [MessageType] VARCHAR (250)    NOT NULL,
    [RecipientInboxWorkQueueUri] VARCHAR(130) NOT NULL, 
    [DateStamp] datetime2(7) NOT NULL default(getutcdate()),
    CONSTRAINT [PK_MessageTypeDispatched] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageType] ASC)
);

