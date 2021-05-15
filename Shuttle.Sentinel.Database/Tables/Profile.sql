CREATE TABLE [dbo].[Profile]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(), 
	[SentinelId] UNIQUEIDENTIFIER NOT NULL,
    [EffectiveFromDate] DATETIME2(7) NOT NULL,
    [EffectiveToDate] DATETIME2(7) NOT NULL,
    [EMailAddress] NVARCHAR(320) NOT NULL, 
    [DateActivated] DATETIME2 NULL, 
    [PasswordResetToken] UNIQUEIDENTIFIER NULL, 
    [PasswordResetTokenDateRequested] DATETIME2 NULL, 
    [SecurityToken] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Profile] PRIMARY KEY NONCLUSTERED (Id) 
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_Profile] ON [dbo].[Profile] ([SentinelId],[EffectiveFromDate])

GO

CREATE INDEX [IX_Profile_EMailAddress] ON [dbo].[Profile] ([EMailAddress], [EffectiveFromDate])

GO

CREATE INDEX [IX_Profile_PasswordResetToken] ON [dbo].[Profile] (PasswordResetToken)

GO

CREATE INDEX [IX_Profile_SecurityToken] ON [dbo].[Profile] ([SecurityToken])
