CREATE TABLE [dbo].[MessageTypeDispatched] (
    [EndpointId]  UNIQUEIDENTIFIER NOT NULL,
    [MessageType] VARCHAR (250)    NOT NULL,
    [RecipientInboxWorkQueueUri] VARCHAR(130) NOT NULL, 
    CONSTRAINT [PK_MessageTypeDispatched] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageType] ASC)
);

