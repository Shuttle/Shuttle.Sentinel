CREATE TABLE [dbo].[MessageTypeHandled] (
    [EndpointId]  UNIQUEIDENTIFIER NOT NULL,
    [MessageType] VARCHAR (250)    NOT NULL,
    CONSTRAINT [PK_MessageTypeHandled] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageType] ASC)
);

