CREATE TABLE [dbo].[InspectionQueue] (
    [SequenceNumber] INT              IDENTITY (1, 1) NOT NULL,
    [SourceQueueUri] VARCHAR (130)    NOT NULL,
    [MessageId]      UNIQUEIDENTIFIER NOT NULL,
    [MessageBody]    VARBINARY (MAX)  NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_InspectionQueue]
    ON [dbo].[InspectionQueue]([MessageId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InspectionQueue_SequenceNumber]
    ON [dbo].[InspectionQueue]([SequenceNumber] ASC);

