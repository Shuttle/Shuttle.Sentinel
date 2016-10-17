CREATE TABLE [dbo].[SystemParameter] (
    [ParameterName]  VARCHAR (65)  NOT NULL,
    [ParameterValue] VARCHAR (130) NOT NULL,
    CONSTRAINT [PK_SystemParameter] PRIMARY KEY CLUSTERED ([ParameterName] ASC)
);

