CREATE TABLE [dbo].[EndpointLogEntry]
(
	[EndpointId]            UNIQUEIDENTIFIER NOT NULL,
	[DateLogged]           DATETIME         NOT NULL,
	[DateRegistered]           DATETIME         NOT NULL,
	[Message]    VARCHAR (1024)    NOT NULL
)

GO

CREATE INDEX [IX_EndpointLogEntry] ON [dbo].[EndpointLogEntry] ([EndpointId], [DateLogged])
