CREATE TABLE [dbo].[EndpointMessageTypeHandled] (
    [EndpointId]  UNIQUEIDENTIFIER NOT NULL,
    [MessageType] VARCHAR (250)    NOT NULL,
    [DateStamp] datetime2(7) NOT NULL default(getutcdate()),
    CONSTRAINT [PK_MessageTypeHandled] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageType] ASC)
);

