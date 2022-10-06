CREATE TABLE [dbo].[EndpointSystemMetric]
(
	[EndpointId]               UNIQUEIDENTIFIER NOT NULL,
	[DateRegistered]           DATETIME2         NOT NULL,
	[Name]              VARCHAR (250)    NOT NULL,
	[Value]                    DECIMAL(18, 5)              NOT NULL,
)

GO

CREATE CLUSTERED INDEX [IX_EndpointSystemMetric] ON [dbo].[EndpointSystemMetric] ([EndpointId], [DateRegistered], [Name])
