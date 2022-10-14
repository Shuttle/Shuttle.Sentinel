CREATE TABLE [dbo].[EndpointMessageTypeAssociation] (
    [EndpointId]            UNIQUEIDENTIFIER NOT NULL,
    [MessageTypeHandled]    VARCHAR (250)    NOT NULL,
    [MessageTypeDispatched] VARCHAR (250)    NOT NULL,
    [DateStamp] datetime2(7) NOT NULL default(getutcdate()),
    CONSTRAINT [PK_MessageTypeAssociation] PRIMARY KEY CLUSTERED ([EndpointId] ASC, [MessageTypeHandled] ASC, [MessageTypeDispatched] ASC)
);

