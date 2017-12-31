CREATE TABLE [dbo].[Macro] (
    [MacroID]     INT              IDENTITY (1, 1) NOT NULL,
    [UserID]      UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (50)    NOT NULL,
    [MacroText]   TEXT             NOT NULL,
    [Description] NVARCHAR (512)   NOT NULL,
    [Created]     DATETIME         NOT NULL,
    [Rank]        INT              NOT NULL,
    [Views]       INT              NOT NULL
);

