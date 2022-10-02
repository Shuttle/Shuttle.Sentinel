CREATE TABLE [dbo].[EndpointMessageTypeAssociation] (
    [EndpointId]            UNIQUEIDENTIFIER NOT NULL,
    [MessageTypeHandled]    VARCHAR (250)    NOT NULL,
    [MessageTypeDispatched] VARCHAR (250)    NOT NULL,
    CONSTRAINT [PK_MessageTypeAssociation] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageTypeHandled] ASC, [MessageTypeDispatched] ASC)
);

