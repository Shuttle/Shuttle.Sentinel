CREATE TABLE [dbo].[SystemUserRole] (
    [UserId]   UNIQUEIDENTIFIER NOT NULL,
    [RoleName] VARCHAR (130)    NOT NULL,
    CONSTRAINT [PK_SystemUserRole_1] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleName] ASC)
);

