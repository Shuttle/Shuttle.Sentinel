CREATE TABLE [dbo].[Schedule] (
    [Name]              VARCHAR (120) NOT NULL,
    [InboxWorkQueueUri] VARCHAR (130) NOT NULL,
    [CronExpression]    VARCHAR (25)  NOT NULL,
    [NextNotification]  DATETIME      NOT NULL,
    CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED ([Name] ASC, [InboxWorkQueueUri] ASC)
);

