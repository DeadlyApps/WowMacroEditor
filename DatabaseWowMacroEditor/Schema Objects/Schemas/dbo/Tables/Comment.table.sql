CREATE TABLE [dbo].[Comment] (
    [CommentID]   INT              IDENTITY (1, 1) NOT NULL,
    [MacroID]     INT              NOT NULL,
    [Text]        NVARCHAR (512)   NOT NULL,
    [Rank]        INT              NOT NULL,
    [UserID]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NOT NULL
);

