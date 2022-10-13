CREATE TABLE [dbo].[EndpointLogEntry]
(
	[EndpointId]            UNIQUEIDENTIFIER NOT NULL,
	[DateLogged]           DATETIME         NOT NULL,
	[DateRegistered]           DATETIME         NOT NULL,
	[Message]    VARCHAR (1024)    NOT NULL,
	[LogLevel] int,
	[Category] varchar(128),
	[EventId] int,
	[Scope] varchar(128)
)

GO

CREATE INDEX [IX_EndpointLogEntry] ON [dbo].[EndpointLogEntry] ([EndpointId], [DateLogged])
