CREATE TABLE [dbo].[SystemMetric]
(
	[EndpointId] UNIQUEIDENTIFIER NOT NULL,
	[DateRegistered] DateTime NOT NULL CONSTRAINT [DF_SystemMetric_DateRegistered] DEFAULT getdate(),
    [Name] VARCHAR(130) NOT NULL, 
    [Value] VARCHAR(130) NOT NULL, 
)

GO

CREATE CLUSTERED INDEX [IX_SystemMetric] ON [dbo].[SystemMetric] ([EndpointId], [DateRegistered])
