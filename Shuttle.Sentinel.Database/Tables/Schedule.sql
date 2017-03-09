CREATE TABLE [dbo].[Schedule]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(120) NOT NULL, 
    [InboxWorkQueueUri] VARCHAR(130) NOT NULL, 
    [CronExpression] VARCHAR(25) NOT NULL, 
    [NextNotification] DATETIME NOT NULL
)

GO

CREATE INDEX [IX_Schedule] ON [dbo].[Schedule] ([Name])
