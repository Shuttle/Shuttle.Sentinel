CREATE TABLE [dbo].[EndpointSubscription]
(
	[EndpointId]            UNIQUEIDENTIFIER NOT NULL,
	[MessageType]       VARCHAR (250) NOT NULL, 
	[DateStamp] datetime2(7) NOT NULL default(getutcdate()),
    CONSTRAINT [PK_EndpointSubscription] PRIMARY KEY ([EndpointId], [MessageType])
)
