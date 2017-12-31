CREATE TABLE [dbo].[UserProfile] (
    [UserID]       UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]  NVARCHAR (250)   NULL,
    [Description]  NVARCHAR (MAX)   NULL,
    [GuildWebsite] NVARCHAR (2048)  NULL,
    [BirthDate]    DATE             NULL,
    [Email]        NVARCHAR (250)   NULL,
    [Location]     NVARCHAR (250)   NULL,
    [UserIntID]    INT              IDENTITY (4500, 1) NOT NULL
);

