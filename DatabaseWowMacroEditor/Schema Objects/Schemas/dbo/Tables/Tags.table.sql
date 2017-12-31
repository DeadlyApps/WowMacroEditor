CREATE TABLE [dbo].[Tags] (
    [TagID]          INT        IDENTITY (1, 1) NOT NULL,
    [Tag]            CHAR (25)  NOT NULL,
    [TagDescription] CHAR (512) NOT NULL,
    [PriorityTag]    BIT        NOT NULL
);

