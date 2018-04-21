CREATE TABLE [dbo].[Queue] (
    [Id]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Uri] VARCHAR (130)    NOT NULL,
    [Processor] VARCHAR(25) NOT NULL DEFAULT 'inbox', 
    [Type] VARCHAR(25) NOT NULL DEFAULT 'work', 
    CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Queue]
    ON [dbo].[Queue]([Uri] ASC);

