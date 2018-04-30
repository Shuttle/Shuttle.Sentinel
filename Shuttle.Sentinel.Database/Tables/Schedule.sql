CREATE TABLE [dbo].[Schedule] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Name]              VARCHAR (120)    NOT NULL,
    [InboxWorkQueueUri] VARCHAR (130)    NOT NULL,
    [CronExpression]    VARCHAR (25)     NOT NULL,
    [NextNotification]  DATETIME         NOT NULL,
    CONSTRAINT [PK_Schedule] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Schedule]
    ON [dbo].[Schedule]([Name] ASC, [InboxWorkQueueUri] ASC, [CronExpression] ASC);

