CREATE TABLE [dbo].[EndpointMessageTypeMetric] (
    [MetricId]                 UNIQUEIDENTIFIER NOT NULL,
    [MessageType]              VARCHAR (250)    NOT NULL,
    [DateRegistered]           DATETIME         NOT NULL,
    [EndpointId]               UNIQUEIDENTIFIER NOT NULL,
    [Count]                    INT              NOT NULL,
    [TotalExecutionDuration]   FLOAT (53)       NOT NULL,
    [FastestExecutionDuration] FLOAT (53)       NOT NULL,
    [SlowestExecutionDuration] FLOAT (53)       NOT NULL,
    CONSTRAINT [PK_MessageTypeMetric] PRIMARY KEY NONCLUSTERED ([MetricId] ASC, [MessageType] ASC)
);




GO
CREATE CLUSTERED INDEX [IX_MessageTypeMetric]
    ON [dbo].[EndpointMessageTypeMetric]([DateRegistered] ASC);



