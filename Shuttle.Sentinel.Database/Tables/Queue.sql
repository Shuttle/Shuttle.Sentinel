CREATE TABLE [dbo].[Queue] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(),
    [Uri] VARCHAR (130) NOT NULL, 
    [DisplayUri] VARCHAR(130) NOT NULL, 
    CONSTRAINT [PK_Queue] PRIMARY KEY ([Id])
);


GO

CREATE INDEX [IX_Queue] ON [dbo].[Queue] ([Uri])
