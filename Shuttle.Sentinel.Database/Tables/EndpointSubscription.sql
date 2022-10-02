CREATE TABLE [dbo].[EndpointSubscription]
(
	[EndpointId]            UNIQUEIDENTIFIER NOT NULL,
	[MessageType]       VARCHAR (250) NOT NULL, 
    CONSTRAINT [PK_EndpointSubscription] PRIMARY KEY ([EndpointId], [MessageType])
)
