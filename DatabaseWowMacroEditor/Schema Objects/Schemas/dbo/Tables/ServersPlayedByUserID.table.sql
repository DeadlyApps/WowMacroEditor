CREATE TABLE [dbo].[ServersPlayedByUserID] (
    [ServersPlayedByUserID] INT              IDENTITY (1, 1) NOT NULL,
    [UserID]                UNIQUEIDENTIFIER NOT NULL,
    [ServerName]            NVARCHAR (512)   NULL,
    [CharacterMainRace]     NVARCHAR (512)   NULL,
    [CharacterMainName]     NVARCHAR (512)   NULL,
    [CharacterSecondName]   NVARCHAR (512)   NULL,
    [CharacterSecondRace]   NVARCHAR (512)   NULL
);

